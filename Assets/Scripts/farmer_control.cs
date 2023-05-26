using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class farmer_control : MonoBehaviour
{
    static Animator anim; 
    public float rotation_speed = 75;
    public float speed = 1200;
    public float jumpSpeed = 300;
    private Rigidbody rb;

    
    public Image healthBarFill;
    public float health = 100f;
    private float maxHealth = 100f;
    public bool isAlive = true;
    public bool isGrounded = true;
    public Game_Over_Screen Game_Over_Screen;
    public bool isInfected = true;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }




    public void updateHealthBar()
    {   
        //Set fill to the amount of current health compared to the player's max health
        healthBarFill.fillAmount = Mathf.Clamp(this.health / this.maxHealth, 0, 1f);

        // Displays Game Over screen if damage is zero or negative
        if(this.health <= 0){
            this.isAlive = false;
            Game_Over_Screen.Display();
            
            //Meant to destroy player object, doesnt work bc of camera:
            //gameObject.SetActive(false);
        }
            
    }

    public void takeDamage(GameObject damageDealer)
    {
        //Get knockback vector base of the position of the player and the damage dealer
        Vector3 knockbackVector = this.transform.position - damageDealer.transform.position;

        //Reduce y vector to prevent popping into the air so much
        knockbackVector.y *= 0.65f;

        //Apply knockback on the player
        rb.AddRelativeForce(knockbackVector * 600);

        //Decrease health
        this.health -= 20f;
        updateHealthBar();

        
    }


    // Update is called once per frame
    void Update()
    {
        if (this.isAlive)
        {
            //Rotation for the animated model
            float rotation = Input.GetAxis("Horizontal") * rotation_speed * Time.deltaTime;

            
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.W))
                {
                    //Apply force to move forward
                    rb.AddRelativeForce(0, 0, speed * Time.deltaTime);
                    anim.SetBool("isWalking", true);
                }

                if (Input.GetKey(KeyCode.S))
                {
                    //Apply force to move backwards
                    rb.AddRelativeForce(0, 0, -speed * Time.deltaTime);
                    anim.SetBool("isWalking", true);
                }



            }
            else
            {
                //Walk if a movement key is being pressed
                anim.SetBool("isWalking", false);
            }
            transform.Rotate(0, rotation, 0);
        }
        else
        {
            //Walk animation only when the player is alive
            anim.SetBool("isWalking", false);
        }

        if (Input.GetKey(KeyCode.Space) && this.isGrounded == true)
        {
            //Apply jumpforce to player
            rb.AddRelativeForce(0, jumpSpeed, 0);

            //Set grounded flag to prevent infinite jumping
            this.isGrounded = false;
        }

        //Kill player if they fall off
        if(rb.position.y < -20)
        {
            this.isAlive = false;
            Game_Over_Screen.Display();
        }


    }



    void FixedUpdate()
    {
        //Get downward vector
        Vector3 downward = transform.TransformDirection(Vector3.down);

        //If there is ground below the player based on a certain distance then the player is grounded
        if (Physics.Raycast(transform.position, downward, 2.5f))
        {
            this.isGrounded = true;
        }
        //If the player is not grounded then they should be in the air so apply extra gravity to prevent long air time
        if(this.isGrounded == false)
        {
            rb.AddForce(downward * 10f);
        }


    }
}
