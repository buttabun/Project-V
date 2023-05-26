using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private Text infectedText;
    private Text goalText;

    void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        infectedText = GameObject.Find("Infected").GetComponent<Text>();
        goalText = GameObject.Find("Goal").GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        //Set UI based on the values in the game controller
        infectedText.text = "Infected: " + GameController.instance.levelInfected;
        goalText.text = "Goal: " + GameController.instance.infectedGoal;
        
    }
}
