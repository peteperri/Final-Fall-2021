using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleRunningGuyController : MonoBehaviour
{
    private Vector2 cornerTopRight = new Vector2(6.54f, 3.05f);
    private Vector2 cornerTopLeft = new Vector2(-10.19f, 3.05f);
    private Vector2 cornerBottomLeft = new Vector2(-10.19f, -4.67f);
    private Vector2 cornerBottomRight = new Vector2(6.54f, -4.67f);
    private bool goingTopRight = false;
    private bool goingTopLeft = false;
    private bool goingBottomLeft = false;
    private bool goingBottomRight = false;
    public float speed;
    private Vector3 rotationVector;

    void Start()
    {
        rotationVector = this.transform.rotation.eulerAngles;
    }
    void FixedUpdate()
    {
        if(cornerTopRight.Equals(new Vector2(transform.position.x, transform.position.y))){
            goingTopLeft = true;
            rotationVector.z = 0;
            transform.rotation = Quaternion.Euler(rotationVector);
            this.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        if(cornerTopLeft.Equals(new Vector2(transform.position.x, transform.position.y))){
           goingBottomLeft = true;
           rotationVector.z = 90; 
           transform.rotation = Quaternion.Euler(rotationVector);       
        }
        if(cornerBottomLeft.Equals(new Vector2(transform.position.x, transform.position.y))){
            goingBottomRight = true;
            rotationVector.z = 0;
            transform.rotation = Quaternion.Euler(rotationVector);
            this.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        if(cornerBottomRight.Equals(new Vector2(transform.position.x, transform.position.y))){
            goingTopRight = true;
            rotationVector.z = -90; 
            transform.rotation = Quaternion.Euler(rotationVector);
        }

        if(goingTopLeft){
            this.transform.position = Vector2.MoveTowards(transform.position, cornerTopLeft, Time.deltaTime * speed);
            //Debug.Log("Moving to Top Left");
            if(new Vector2(transform.position.x, transform.position.y).Equals(cornerTopLeft)){
                goingTopLeft = false;
            }
        }
        if(goingBottomLeft){
            this.transform.position = Vector2.MoveTowards(transform.position, cornerBottomLeft, Time.deltaTime * speed);
            //Debug.Log("Moving to Bottom Left");
            if(new Vector2(transform.position.x, transform.position.y).Equals(cornerBottomLeft)){
                goingBottomLeft = false;
            }
        }
        if(goingBottomRight){
            this.transform.position = Vector2.MoveTowards(transform.position, cornerBottomRight, Time.deltaTime * speed);
            //Debug.Log("Moving to Bottom Right");
            if(new Vector2(transform.position.x, transform.position.y).Equals(cornerBottomRight)){
                goingBottomRight = false;
            }
        }

        if(goingTopRight){
            this.transform.position = Vector2.MoveTowards(transform.position, cornerTopRight, Time.deltaTime * speed);
            //Debug.Log("Moving to Top Right");
            if(new Vector2(transform.position.x, transform.position.y).Equals(cornerTopRight)){
                goingTopRight = false;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
        {

            RubyController player = other.gameObject.GetComponent<RubyController>();

            if(player != null){
                Debug.Log("Collided with the player");
                player.unlimitedAmmo = true;
                player.ammoText.text = "Gears: ∞";
                Destroy(this.gameObject);
            }


        }
}