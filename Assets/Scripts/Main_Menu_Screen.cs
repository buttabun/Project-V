using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu_Screen : MonoBehaviour
{
    public void Start_Game() {
        SceneManager.LoadScene("Assignment4");
    }

    public void Load_Credits() {
        SceneManager.LoadScene("Credits");
    }

    public void Quit_Game() {
        Application.Quit();
        Debug.Log("Game closed");
    }    
}
