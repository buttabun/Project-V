using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits_Script : MonoBehaviour
{
    public void Exit_Credits()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
