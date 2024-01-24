using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Spilk : MonoBehaviour
{
    // Start is called before the first frame update

    public float bounceSpeed;
    private float baseY;
    float deadCount;

    BoxCollider2D coll;

    Animator anim;
    void Start()
    {
        deadCount=0;
        anim = GetComponent<Animator>();
        baseY = transform.position.y;
        coll = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        deadCount+=Time.deltaTime;

        if(deadCount>=1.25f){
            comeBack();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if(other.CompareTag("Player")){
            deadCount=0;
            Player2.Instance.refill(this);
            coll.enabled=false;
            anim.SetBool("Gone",true);
            //Destroy(this);
        }

    }
    void FixedUpdate()
    {
        transform.position=new Vector2(transform.position.x,baseY+(float) Math.Sin((Time.frameCount)/bounceSpeed)/4);
        
        
    }
    public void comeBack(){
        coll.enabled=true;
        anim.SetBool("Gone",false);
    }

}
