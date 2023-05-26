using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterController))]

public class Villager_AI : MonoBehaviour
{
    //Bool to state if we are infected
    public bool isInfected;

    //Post string for our current state
    public string state = "Current State";

    //Attack Distance, Infection time, and deathTime
    public float AttackDist = 2f;
    public float infectionTime = 1f;
    public float deathTime = 5f;

    //Controls for Wandering behavouir 
    public float maxWanderTime;
    public float minWanderTime;
    public float wanderRadius;

    //Color for us to start as and one to change to when infected
    public Color default_Color;
    public Color infected_Color;


    //Private feilds
    private float workingTimer;
    private NavMeshAgent agent;
    private GameObject primaryFocus;
    private List<GameObject> neighbors = new List<GameObject>();
    private Vector3 Goal;
    private float wanderTime;


    private float infection = 0f;
    private GameObject goal;
    private farmer_control playerInstance;
    private float maxInfection = 25f;

    public int infected_count = 0;
    public Image healthBarFill;
    // Start is called before the first frame update

    private void Start()
    {
        goal = GameObject.Find("farmer_animated");
        playerInstance = goal.gameObject.GetComponent<farmer_control>();
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();


        //If we don't have a sphearCollider we'd want to add one
        if (GetComponent<SphereCollider>() == null)
        {
            SphereCollider newCollider = gameObject.AddComponent<SphereCollider>();
            newCollider.isTrigger = true;
            newCollider.center = new Vector3(0f, .55f, 0f);
            newCollider.radius = 5f;
        }

        foreach (Renderer render in GetComponentsInChildren<Renderer>())
        {
            render.material.SetColor("_Color", default_Color);
        }

        //No matter what we want to wander first
        Wander();


    }

    // Update is called once per frame

    public void updateInfectionBar()
    {
        healthBarFill.fillAmount = Mathf.Clamp(this.infection / this.maxInfection, 0, 1f);
    }

    private void Update()
    {
        if (Vector3.Distance(this.transform.position, goal.transform.position) < 10 && playerInstance.isAlive)
        {
            this.infection += 3 * Time.deltaTime;
            updateInfectionBar();
        }
        if (this.infection > this.maxInfection)
        {
            GameController.instance.addInfected();
            Destroy(gameObject);

            //**************
            infected_count++;
        }
    }

    void FixedUpdate()
    {

        if (InfectedInRange()) // If an infected is in range
            Flee(); // Run away!!!
        else
            Wander(); // If not just wander

        //Debug path drawing
        if (isInfected)
            Debug.DrawLine(transform.position, agent.destination, Color.red);
        else
            Debug.DrawLine(transform.position, agent.destination, Color.green);
    }

    private IEnumerator WaitTime(float time) //Stops agent and waits for time before unstopping
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        yield return new WaitForSeconds(time);
        agent.isStopped = false;
    }

    private IEnumerator DeathTimer(float time) //Waits for time before setting self to inactive
    {
        yield return new WaitForSeconds(time);
        //Destroy(gameObject); // Destroying a gameobject that other villagers are watching lead to some errors 
        gameObject.SetActive(false);
    }

    private void Chase() //Chase a target!
    {
        state = "Chasing Target " + primaryFocus.name;  //Change our state to chasing and the target name
        agent.SetDestination(primaryFocus.transform.position); //Set the agents destination to its primary target
        if (Vector3.Distance(transform.position, primaryFocus.transform.position) <= AttackDist)
        {
            SpreadInfection(); // if in range spreadInfection
        }

    }
    private void Flee()//Run away from an infected!
    {
        state = "Fleeing Infected " + primaryFocus.name; //Change our state to Fleeing from and the target name
        Vector3 VectorToInfected = transform.position - primaryFocus.transform.position;
        Vector3 moveVector = transform.position + VectorToInfected; // Calculate position oppisite to the infected
        agent.SetDestination(moveVector); //setDestination to that location
    }

    private void SpreadInfection()
    {
        primaryFocus.GetComponent<Villager_AI>().BecomeInfected(); //Call the other villagers Become Infected method
        primaryFocus = null; //Set target to null
        StartCoroutine(WaitTime(infectionTime)); //Wait for sometime before moving again

    }

    public void BecomeInfected()
    {
        //Set tag and bool
        tag = "Infected";
        isInfected = true;

        //Start wait timer and death timer
        StartCoroutine(WaitTime(infectionTime));
        StartCoroutine(DeathTimer(deathTime));

        // If you have a trail renderer compoent
        //GetComponent<TrailRenderer>().enabled = true;

        //Set Colors
        foreach (Renderer render in GetComponentsInChildren<Renderer>())
        {
          render.material.SetColor("_Color", infected_Color);
        }

        //Set Attributes
        agent.speed *= 20f;
        agent.acceleration *= 20f;
        agent.angularSpeed = 1000f;
        agent.stoppingDistance = 0;
        minWanderTime = .5f;
        maxWanderTime = 1f;
        GetComponent<SphereCollider>().radius *= 4f;

    }

    private bool InfectedInRange()
    {
        bool foundInfected = false;
        foreach (GameObject neighbor in neighbors)
        {
            if(neighbor == null)
            {
                continue;
            }
            if (neighbor.active && neighbor.tag == "Infected")
            {
                foundInfected = true;
                if (primaryFocus == null || (Vector3.Distance(transform.position, primaryFocus.transform.position) >
                Vector3.Distance(transform.position, neighbor.transform.position)))
                    primaryFocus = neighbor;

            }
        }
        if (foundInfected == false)
            primaryFocus = null;

        return foundInfected;

    }


    void Wander()
    {
        state = "Wandering"; //Post state to string



        workingTimer += Time.deltaTime; // Add to timer
        if (workingTimer >= wanderTime) // If timer is larger than our set wanderTime start wandering
        {
            Goal = Random.insideUnitSphere * wanderRadius + transform.position; //Get random position
            Goal.y = transform.position.y; // set this positions y value to our own (else you'll have a bunch of floating goals)
            agent.SetDestination(Goal); // Move
            workingTimer = 0; // Set timer to zero
            SetWanderTime(); //Roll a random time to move again
            agent.updateRotation = true;
        }
    }
    void SetWanderTime()
    {
        wanderTime = Random.Range(minWanderTime, maxWanderTime);
    }


    void OnTriggerEnter(Collider other)
    {
        //If something enters the collider add it to a list of objects
        if (!neighbors.Contains(other.gameObject))
        {
            neighbors.Add(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        //If something leaves the collider remove it from the list
        if (neighbors.Contains(other.gameObject))
        {
            neighbors.Remove(other.gameObject);
        }
    }
}   