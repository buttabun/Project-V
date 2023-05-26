using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour

    
{
    public float health = 100f;
    private float maxHealth = 100f;
    public float speed = 1200;
    private Rigidbody rb;
    public Image healthBarFill;
    public bool isAlive = true;
    public bool isInfected = true;

    //**************
    public Game_Over_Screen Game_Over_Screen;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        

        
    }

    public void updateHealthBar()
    {
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
        Vector3 knockbackVector = this.transform.position - damageDealer.transform.position;
        rb.AddRelativeForce(knockbackVector * 600);
        this.health -= 20f;
        updateHealthBar();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isAlive)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddRelativeForce(0, 0, speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.S))
            {
                rb.AddRelativeForce(0, 0, -speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.A))
            {
                rb.AddRelativeForce(-speed * Time.deltaTime, 0, 0);
            }

            if (Input.GetKey(KeyCode.D))
            {
                rb.AddRelativeForce(speed * Time.deltaTime, 0, 0);
            }
        }
        
            


    }
}
