using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_Over_Screen : MonoBehaviour
{
   //public Text Infected_Text;

    public void Display()
    {
        gameObject.SetActive(true);
        //Infected_Text.text = "Infected: " + infected_count.ToString();
    }
      
    public void Restart_Button()
    {
        SceneManager.LoadScene("Assignment4");
    }

    public void Exit_Button()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
