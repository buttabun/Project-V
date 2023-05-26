using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private Renderer mr;
    private GameObject goal;
    private float infection = 0f;
    private float maxInfection = 25f;
    private GameObject attackBox;
    private float attackTimer = 0.50f;
    public Image healthBarFill;
    private farmer_control playerInstance;

    //*****

    // Start is called before the first frame update
    void Start()
    {
        //Get player object
        goal = GameObject.Find("farmer_animated");

        //Get enemy nav agent
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        //Get player script
        playerInstance = goal.gameObject.GetComponent<farmer_control>();



    }

    public void updateInfectionBar()
    {
        healthBarFill.fillAmount = Mathf.Clamp(this.infection / this.maxInfection, 0, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        //Increase infection based on if the player is close enough
        if (Vector3.Distance(this.transform.position, goal.transform.position) < 10 && playerInstance.isAlive)
        {
            this.infection += 1 * Time.deltaTime;
            updateInfectionBar();
        }
        //If the player is close enough start the attack time to attack
        if(Vector3.Distance(this.transform.position, goal.transform.position) < 3 && playerInstance.isAlive)
        {
            attackTimer -= 1 * Time.deltaTime;
            if(attackTimer <= 0)
            {
                //Call take damage method on player that will decrease their helth and apply knockback
                playerInstance.takeDamage(this.gameObject);
                //Reset attack timer
                attackTimer = 0.50f;
            }

        }
        else
        {
            //If the player moves out of range increase the attack time so the player has more time
            attackTimer = 0.75f;
        }
        if(this.infection > this.maxInfection)
        {
            //If the enemey maxes out their infection then add to the level tracker for infected and destroy the object
            GameController.instance.addInfected();
            Destroy(gameObject);

        }
        
        
    }

    void FixedUpdate()
    {
        //Enemy will chase the player but only if they are still alive
        if(playerInstance.isAlive == true)
        {
            agent.destination = goal.transform.position;
        }
        
    }
}
