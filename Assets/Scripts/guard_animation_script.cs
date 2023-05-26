using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guard_animation_script : MonoBehaviour
{
    static Animator anim; 
    public float speed = 500;
    public float rotation_speed = 75;
    private float infection = 0f;
    private float maxInfection = 25f;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator> ();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis ("Vertical")*speed; 
        float rotation = Input.GetAxis ("Horizontal")*rotation_speed;
        x *= Time.deltaTime;
        rotation *= Time.deltaTime;


        transform.Translate (0, 0, x);
        transform.Rotate (0, rotation, 0);

        if (x == 0) {
            anim.SetBool("isWalking", false);
        } else {
            anim.SetBool("isWalking", true);
        }

    }
}
