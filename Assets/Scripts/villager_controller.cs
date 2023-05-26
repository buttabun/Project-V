using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class villager_controller : MonoBehaviour
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
        //Get horizontal input (W A)
       /* float x = Input.GetAxis("Horizontal");

        //Get vertical input (A D)
        float z = Input.GetAxis("Vertical"); 

        Vector3 dirVector = new Vector3(x, 0, z);

        dirVector = dirVector*speed;

        //Model rotation:
        if (Math.Abs(x) > .3f || Math.Abs(z) > .3f)
        {
            Quaternion newRot = Quaternion.LookRotation(new Vector3(x, 0f, z));
            
        }*/

        float x = Input.GetAxis ("Vertical")*speed; 
        float rotation = Input.GetAxis ("Horizontal")*rotation_speed;
        x *= Time.deltaTime;
        rotation *= Time.deltaTime;


        transform.Translate (0, 0, x);
        transform.Rotate (0, rotation, 0);

        if (x != 0) {
            anim.SetBool("IsIdle", false);
        } else {
            anim.SetBool("IsIdle", true);
        }

        if (Vector3.Distance(this.transform.position, player.transform.position) < 15)
        {
            this.infection += 3 * Time.deltaTime;
        }

        if(this.infection > this.maxInfection)
        {
            //TO DO: Increase global infection count
            Destroy(gameObject);
        }
    }
}
