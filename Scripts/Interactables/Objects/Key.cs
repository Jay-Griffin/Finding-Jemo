using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Key : MonoBehaviour
{

    public int color;

    public float dist, movModify, bounceSpeed, randomFradd;
    bool collected;

    Vector2 start;

    private float targetX, targetY;

    float baseY;
    public BoxCollider2D Physical;
    int count;

    
    public void setTargetLoc(float x, float y){
        targetX=x;
        targetY=y;
    }

    public Transform getTransform(){
        return transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        count=0;
        start = transform.position;
        System.Random rnd = new System.Random();
        randomFradd=rnd.Next(0,100);

        baseY=transform.position.y;

        collected=false;
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if(other.CompareTag("Player")&!collected){
            collected=true;
            Player2.Instance.giveKey(this);
           //Debug.Log("gave Key");
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        count++;
         Player2.Instance.ignoreCollision(Physical,true);
        
        
        if(collected){
            transform.position=new Vector3(transform.position.x+(targetX)/movModify,transform.position.y+(targetY)/movModify,0);
        }else{
            transform.position=new Vector2(transform.position.x,baseY+(float) Math.Sin((Time.frameCount)/bounceSpeed)/4);
            // transform.position=new Vector3(transform.position.x,transform.position.y+(float) Math.Sin((count+randomFradd)/bounceSpeed)/1-(float)Math.Sin((count-1+randomFradd)/bounceSpeed)/1,0);
        }
        
        
    }

    public void Reset(){
        transform.position=start;
        collected=false;
    }
}
