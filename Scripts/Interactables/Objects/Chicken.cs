using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chicken : MonoBehaviour
{
    Dictionary<Action,List<Action>> actionMap = new Dictionary<Action,List<Action>>();
    public enum Action{
        IdleLeft = 0,
        IdleRight = 1,
        IdleStraight = 2,
        WalkLeft = 3,
        WalkRight = 4,
        Jump = 5,
        Dance = 6,
        SitOnPlayer = 7
    }

    //public Action testAct;
    public float speed, selfRightingForce, jumpVel;
    public int minJumpRot, maxJumpRot; 

    int pauseGrounded;

    public LayerMask groundLayer, crumbleLayer;
    public Transform groundCheck;
    bool grounded;
    public Action cAction;
    private int actionDurration;
    private int framesInAction;
     Rigidbody2D bd;

    Animator anim;

    BoxCollider2D coll;
    CircleCollider2D circle;

    System.Random rnd;

    Transform Sprite;
    
    // Start is called before the first frame update
    void Start()
    {   
        rnd = new System.Random();
        instActionMap();
        initialChainage();
        cAction=Action.IdleStraight;
        //Debug.Log(actionMap);


        pauseGrounded=0;

        grounded=false;

        Sprite = transform.Find("Sprite");

        anim = Sprite.GetComponent<Animator>();

        bd=GetComponent<Rigidbody2D>();
        coll=GetComponent<BoxCollider2D>();
        circle=GetComponent<CircleCollider2D>();

        
        
        
    }

    void initialChainage(){
        addActionToAction(Action.IdleStraight,Action.IdleStraight,2100);
        addActionToAction(Action.Jump,Action.IdleStraight,1);
        addActionToAction(Action.WalkLeft,Action.IdleStraight,3);
        addActionToAction(Action.WalkRight,Action.IdleStraight,3);

        addActionToAction(Action.WalkLeft,Action.WalkLeft,125);
        //addActionToAction(Action.IdleLeft,Action.WalkLeft,1);
        addActionToAction(Action.IdleStraight,Action.WalkLeft,1);

        addActionToAction(Action.WalkRight,Action.WalkRight,125);
        //addActionToAction(Action.IdleRight,Action.WalkRight,1);
        addActionToAction(Action.IdleStraight,Action.WalkRight,1);
    }

    void instActionMap(){
        foreach (Action action in (Action[]) Enum.GetValues(typeof(Action)))
        {
            actionMap.Add(action,new List<Action>());
        }
    }
    void addActionToAction(Action toAdd, Action toAddTo, int repeat = 1){
        for(int i=0;i<repeat;i++){
            actionMap[toAddTo].Add(toAdd);
        }
    }

    int remActionFromAction(Action toRem, Action toRemFrom, int repeat = 1){
        int toReturn = 0;
        for(int i=0;i<repeat;i++){
            if(!actionMap[toRemFrom].Remove(toRem)){
                toReturn++;
            }
        }
        return toReturn;
    }

    // Update is called once per frame
    void Update()
    {
        if(pauseGrounded<0){
        grounded= Physics2D.OverlapCircle(groundCheck.position,0.6f,groundLayer)|Physics2D.OverlapCircle(groundCheck.position,0.6f,crumbleLayer);
        }
        anim.SetBool("Grounded", grounded);
        //Debug.Log(grounded);

        anim.SetFloat("Speed",Math.Abs(bd.velocity.x));
        
        
    }

    private void FixedUpdate() {
        pauseGrounded--;

        //chickenAi();
        execute(chickenAi());

        //execute(testAct);


        if(grounded){
            
            anim.SetFloat("Rot", 0);
            float offset = 5;
            float force = selfRightingForce;
            Vector3 point = transform.TransformPoint(offset * Vector3.up);
            bd.AddForceAtPosition(force * Vector3.up, point);

            if(bd.velocity.x>0.1){
                transform.rotation = Quaternion.Euler(0,180,0);
                
                //Sprite.transform.rotation = Quaternion.Euler(0,180,0);
            }else if(bd.velocity.x<-0.1){
                transform.rotation = Quaternion.Euler(0,0,0);
                
                //Sprite.transform.rotation = Quaternion.Euler(0,0,0);
            }
            
        }

       
        Sprite.transform.rotation = Quaternion.Euler(0,0,0);
    }

    void execute(Action toEx){
        //Debug.Log(toEx);
        if(grounded){
            switch(toEx){
                case(Action.IdleLeft):
                    anim.SetInteger("ActionMode",(int) Action.IdleLeft);

                break;
                case(Action.IdleRight):
                    anim.SetInteger("ActionMode",(int) Action.IdleRight);

                break;
                case(Action.IdleStraight):
                    anim.SetInteger("ActionMode",(int) Action.IdleStraight);

                break;
                case(Action.WalkLeft):
                    anim.SetInteger("ActionMode",(int) Action.WalkLeft);
                    //transform.rotation = Quaternion.Euler(0,0,0);
                    bd.AddForce(speed*Vector2.left);
                break;
                case(Action.WalkRight):
                    anim.SetInteger("ActionMode",(int) Action.WalkRight);
                    //transform.rotation = Quaternion.Euler(0,180,0);
                    bd.AddForce(speed*Vector2.right);

                break;
                case(Action.Dance):
                    anim.SetInteger("ActionMode",(int) Action.Dance);

                break;
                case(Action.Jump):
                    anim.SetInteger("ActionMode",(int) Action.Jump);
                    
                    int mod=1;
                    if(rnd.NextDouble()>0.5){
                        mod=-1;
                    }
                    //bd.AddTorque(rnd.Next(minJumpRot,maxJumpRot)*mod);
                    anim.SetFloat("Rot", UnityEngine.Random.Range(minJumpRot,maxJumpRot)*mod);
                    bd.velocity= new Vector2(bd.velocity.x,jumpVel);
                    toEx=cAction;
                    //testAct=Action.IdleStraight;
                    pauseGrounded=100;
                    grounded=false;

                break;
            }

            cAction=toEx;
        }

    }

    Action chickenAi(){
       return actionMap[cAction][rnd.Next(actionMap[cAction].Count)];

    }
}