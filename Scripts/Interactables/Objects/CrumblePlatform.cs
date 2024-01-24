using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblePlatform : MonoBehaviour
{

    private Collider2D collider;

    private int disabledCount, crumbleCount;

    public int timeToRespawn, timeToCrumble;

    private bool Colliding;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        disabledCount=0;
        crumbleCount=0;
        if(timeToRespawn==0){
            timeToRespawn=100;
        }
        if(timeToCrumble==0){
            timeToCrumble=150;
        }
        Colliding=false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        

        //Debug.Log(collider.enabled);
        if(collider.enabled==false){
            //Debug.Log("colliderDisabled");
            disabledCount++;
        }

        if(disabledCount==timeToRespawn|Player2.Instance.dead){
            disabledCount=0;
            crumbleCount=0;
            collider.enabled=true;
        }

        if(crumbleCount>timeToCrumble/4*4){
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
        }else if(crumbleCount>timeToCrumble/4*3){
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(true);
        }else if(crumbleCount>timeToCrumble/4*2){
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(false);
        }else if(crumbleCount>timeToCrumble/4*1){
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
        }else if(crumbleCount>=timeToCrumble/4*0){
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
        }

        if(collider.enabled==false&crumbleCount<=timeToCrumble){
            crumbleCount+=timeToCrumble/30;
        }else if(collider.enabled==true&crumbleCount>0&!Colliding){
            crumbleCount--;
        }

        

        //listen for collider disable
    }
/// <summary>
/// Sent each frame where a collider on another object is touching
/// this object's collider (2D physics only).
/// jay smelly!!! team KING IAN 4 lifer!!!!!! cha CHINGGGGGGGG!!!!!!!
/// </summary>
/// <param name="other">The Collision2D data associated with this collision.</param>
///
void OnCollisionStay2D(Collision2D other)
{
   
    if(other.rigidbody.CompareTag("Player")){
         Colliding=true;
    if(crumbleCount>=timeToCrumble){
        collider.enabled=false;
    }
    if(Player2.Instance.getGrounded()|Player2.Instance.getWalled()){
        if(Input.GetAxisRaw("Crouch")>0|Input.GetAxisRaw("Walk")>0){
            crumbleCount++;
        }else{
            crumbleCount+=3;
        }

        // if(Input.GetButtonDown("Jump")|Input.GetButtonDown("Dash")){
        //     collider.enabled=false;
        // }
    }
    }
    //get player info-evaluate decay rate
}
private void OnCollisionExit2D(Collision2D other) {
    //Debug.Log("Here");
    if(other.rigidbody.CompareTag("Player")){
        Colliding=false;
    }
}
}
