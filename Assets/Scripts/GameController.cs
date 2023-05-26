using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameObject playerInstance;
    public GameObject guard;
    public int levelInfected = 0;
    public int spawnThresh = 2;
    public int infectedGoal = 5;
    public bool canSpawn = false;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            print("Duplicated Game Controller");
        }
        playerInstance = GameObject.Find("farmer_animated");
        print(SceneManager.GetActiveScene().name);
    }

    public void addInfected()
    {
        instance.levelInfected++;
        //Check if goal has been met
        if(instance.levelInfected >= instance.infectedGoal)
        {
            //If on the last level then go to the main menu otherwise progress to the next
            if(SceneManager.GetActiveScene().name == "Town Market")
            {
                SceneManager.LoadScene("Main_Menu");
            }
            else
            {
                print("Level Complete");
                //Load next level scene
                SceneManager.LoadScene("Town Market");
            }
            
        }
        //Only allow spawning once the infection has incremented
        canSpawn = true;

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Spawn guards after two infected then disable enemy spawning to prevent infinite spawning
        if((levelInfected % spawnThresh) == 0 && canSpawn)
        {
            //Spawn guard based on random location around the player
            Vector3 spawnLoc = playerInstance.transform.position;
            spawnLoc.x += Random.Range(-5, 5);
            spawnLoc.z += Random.Range(-5, 5);
            Instantiate(guard, spawnLoc, playerInstance.transform.rotation);
            //Disable spawning to only allow once 
            canSpawn = false;
        }
        
    }
}
