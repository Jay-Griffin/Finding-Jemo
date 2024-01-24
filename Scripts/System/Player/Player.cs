using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Player : MonoBehaviour
{

    bool dead;
    public int freeze{get ;private set;}
    public static Player Instance {get; private set;}

    public float x{get {return transform.position.x;}}
    public float y{get {return transform.position.y;}}
    public int getDir{get {return dir;}}

    public bool jumpPress{get; private set;}
    public bool crouched{get; private set;}
    public bool dashPress{get; private set;}
    public bool walking{get; private set;}
    public bool grounded{get; private set;}
    public bool dashing{get; private set;}

    public bool dispDial{get; set;}
    public string[] Dialogue{get; set;}
    int textIndex;

    public bool moving(){
        return rb.velocity.x!=0;
    }
   // public bool jumpPress{get; private set;}
    
    [SerializeField] private GameObject dial;
    [SerializeField] private TMP_Text dialText;




    public List<Key> getKeys{get {return keys;}}

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        Coll= GetComponent<BoxCollider2D>();
        if(Instance!=null && Instance!=this){
            Destroy(this);
        }else{
            Instance=this;
            //Debug.Log(Instance);
        }
    }


    public GameObject chicken, sprite;
    public int wallMaxStam;
    float groundCheckRad, wallCheckRad, crumbleCheckRad;
    public float velDeAccelW, velDeAccelS, velDeAccelC, jumpVel, dashVel, dashFrames, gAcc, aAcc, fricDeAccel, airDeAccel, deAccelThresh, wallClDeAcel, wallSlAcc, floatGScale;

    bool walled, isFloating;
    public bool canWall, canDash, canFloat;

    int dir, dashes, jumps, dashFrameCount, wallStam, jumpFrames;

    public int dashRefillNum, jumpRefillNum, maxJumpFrames;

    public Transform wallCheck;

    public GameObject groundCheckCont;

    //public BoxCollider2D defColl, crouchColl;
    BoxCollider2D Coll; 
     Vector2 defCollOff, defCollSize, crouchCollOff, crouchCollSize, dashCollOff, dashCollSize;

    public Rigidbody2D rb {get; private set;}

    public LayerMask groundLayer, crumbleLayer;

    private List<Key> keys;
    private List<Spilk> spilks;

    public Vector2? teleport;
    
    Animator anim;

    //Dash DASH;
    System.Random rnd;

    [SerializeField] private bool jumped;
    void Start()
    {
        Physics2D.IgnoreLayerCollision(9,3,true);
        Physics2D.IgnoreLayerCollision(9,8,true);
        Physics2D.IgnoreLayerCollision(9,9,true);
        Physics2D.IgnoreLayerCollision(8,8,true);
        //Physics2D.IgnoreLayerCollision(3,8,true);
        // Physics2D.IgnoreLayerCollision(3,10,true);

        rnd = new System.Random();

        anim = sprite.GetComponent<Animator>();

        keys =new List<Key>();
        spilks =new List<Spilk>();

        //crouchColl.enabled=false;
        
        
        defCollOff = new Vector2(0.00f,-0.24f);
        defCollSize = new Vector2(0.75f,2.54f);

        crouchCollOff = new Vector2(0.00f, -0.77f);
        crouchCollSize = new Vector2(0.75f, 1.47f);

        dashCollOff = new Vector2(0.00f,-0.24f);
        dashCollSize = new Vector2(0.75f,2.0f);
       

        dir=dashes=jumps=1;
        dashFrameCount=0;

        groundCheckRad=0.02f;
        wallCheckRad=0.05f;
        crumbleCheckRad=0.04f;

        walking=jumpPress=dashPress=grounded=walled=crouched=dashing=dead=jumped=false;

        jumpFrames = 0;
        dispDial=false;
        
    }

    void Update(){
        //Debug.Log(jumps);
        
        if(Input.GetButtonDown("Dash")&!dashPress&dashes>0&canDash&freeze<0){
            dashPress=true;
           if(Input.GetAxisRaw("Horizontal")>0&!dashPress){
            dir=1;
            transform.rotation = Quaternion.Euler(0,0,0);

        }else if(Input.GetAxisRaw("Horizontal")<0&!dashPress){
            dir=-1;
            transform.rotation = Quaternion.Euler(0,180,0);

        }
           // Debug.Log(dashPress);
        }
        //grounded= CheckGrounded();
        anim.SetFloat("Speed",Mathf.Abs(rb.velocity.x));

        if(jumped == false)
            jumped = Input.GetButtonDown("Jump");

        if(jumped == true)
            jumped = !Input.GetButtonUp("Jump");

        //Debug.Log(jumped);

        dial.SetActive(dispDial);
        if(dispDial){
            freeze=20;
            rb.velocity=new Vector2(0,0);
            Camera.Instance.textZoom(0.6f);

            dialText.text=Dialogue[textIndex];
            if(Input.GetKeyDown("return")){
                textIndex++;
            }
            if(textIndex==Dialogue.Length){
                dispDial=false;
            }
        }else{
            textIndex=0;
        }
        anim.SetBool("JumpPress", jumpPress);
        anim.SetBool("Grounded", grounded);
    }
    // Update is called once per frame

    void FixedUpdate(){
        grounded= CheckGrounded();
        
        if(Input.GetAxisRaw("Jump")>0){
            jumpFrames++;
            if(grounded){
                jumpPress=true;
            }
            if(jumpFrames>maxJumpFrames){
                jumpPress=false;
            }
        }else{
            jumpPress=false;
        }



        




        rb.gravityScale=9;
        //grounded
        freeze--;
        //Debug.Log(dead);
    if(freeze<0){
        dead=false;
        if(teleport!=null){
            transform.position=(Vector2) teleport;
            teleport=null;
        }
        if(!grounded&Input.GetAxisRaw("Jump")>0&rb.velocity.y<0&canFloat&!crouched&!walled){
            if(!isFloating){
                rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y/9);
                isFloating=true;
            }
            rb.gravityScale=floatGScale;
            
        }
        if(isFloating&(grounded|Input.GetAxisRaw("Jump")==0|walled)){
            isFloating=false;
            GameObject cgo = Instantiate(chicken, transform.position, Quaternion.Euler(0,0,0));
            Rigidbody2D crb = cgo.GetComponent<Rigidbody2D>();
            crb.velocity = new Vector2(rb.velocity.x,0);
            //crb.transform.rotation=Quaternion.Euler(0,0,0);
            crb.transform.position = new Vector2(transform.position.x,transform.position.y+2f);
            
            int mod=1;
            if(rnd.NextDouble()>0.5){
                mod=-1;
            }
            cgo.transform.Find("Sprite").GetComponent<Animator>().SetFloat("Rot", UnityEngine.Random.Range(1,10)*mod);
            //crb.angularVelocity = rnd.Next(7500,7510)*mod;
        }
        
        //walled
        walled= (Physics2D.OverlapCircle(wallCheck.position, wallCheckRad, groundLayer)|Physics2D.OverlapCircle(wallCheck.position, wallCheckRad, crumbleLayer))&!grounded;
        //Debug.Log(walled);

        if(!jumped&((grounded)|(walled&canWall&wallStam>0))){
            jumps=jumpRefillNum;
            dashes=dashRefillNum;
            jumpFrames=0;
            //jumpPress=false;
        }

        if(grounded){
            wallStam=wallMaxStam;
        }

        if(walled&canWall){
            wallStam--;
        }

        //dir
        if(Input.GetAxisRaw("Horizontal")>0&!dashPress){
            dir=1;
            transform.rotation = Quaternion.Euler(0,0,0);

        }else if(Input.GetAxisRaw("Horizontal")<0&!dashPress){
            dir=-1;
            transform.rotation = Quaternion.Euler(0,180,0);
        }

        if(dashPress){
            
            if(Dash()){
                rb.gravityScale=9;
                dashPress=false;
                dashes--;   
                anim.SetBool("Dashing", false);

                crouched = true;
                Coll.size=crouchCollSize;
                Coll.offset=crouchCollOff;
            }
        }else{
            //Debug.Log(walled);
            if(walled&canWall&wallStam>0){
                if(jumpPress&jumpFrames<maxJumpFrames){
                    rb.velocity=new Vector2(jumpVel*-dir,jumpVel);
                    //rb.AddForce(Vector2.up);
                    //rb.AddForce(Vector2.right*dir);



                    
                    
                }else{
                if(Input.GetAxisRaw("Crouch")!=0){
                    rb.velocity=new Vector2(rb.velocity.x,rb.velocity.y-wallSlAcc);
                }else{

                if(Input.GetAxisRaw("Horizontal")==dir){
                    rb.velocity=new Vector2(rb.velocity.x,0);
                    rb.gravityScale=0;
                }else if(rb.velocity.y>0){
                    rb.gravityScale=9;
                    rb.velocity=new Vector2(rb.velocity.x,rb.velocity.y-wallClDeAcel);
                }else{
                    rb.gravityScale=9;
                    rb.velocity=new Vector2(rb.velocity.x,rb.velocity.y+wallClDeAcel);
                }
                }
                rb.velocity=new Vector2(rb.velocity.x,Math.Max(-8,rb.velocity.y));
                
                }
                //Debug.Log(rb.velocity.y);
            }else{
                if(jumpPress&jumpFrames<maxJumpFrames){
                    //rb.AddForce(Vector2.up*jumpVel);
                    //raycast up
                    LayerMask mask = LayerMask.GetMask("Ground");
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, mask); //BONK code
                    if (hit.collider != null)
                    {
                        
                        if ((hit.distance <= 1.25f&!crouched)|(crouched&hit.distance<=.25f))
                        {
                            rb.velocity = new Vector2(rb.velocity.x, -jumpVel/4);
                            jumpFrames=maxJumpFrames;
                        }else{
                            if(!crouched){
                                rb.velocity = new Vector2(rb.velocity.x, jumpVel);
                            }else {
                                rb.velocity = new Vector2(rb.velocity.x,jumpVel/2);
                            }
                        }
                    }else{
                        if(!crouched){
                            rb.velocity = new Vector2(rb.velocity.x, jumpVel);
                        }else {
                            rb.velocity = new Vector2(rb.velocity.x,jumpVel/2);
                        }
                    }
                    





                }
                if(Input.GetAxisRaw("Horizontal")!=0){

                            if(grounded)
                                rb.velocity=new Vector2(rb.velocity.x+gAcc*dir,rb.velocity.y);
                            else
                                rb.velocity=new Vector2(rb.velocity.x+aAcc*dir,rb.velocity.y);
                            
                }
            }
            //deaccel
            if(Math.Abs(rb.velocity.x)<deAccelThresh){
                rb.velocity= new Vector2(0,rb.velocity.y);
            }else if(grounded){
                if(rb.velocity.x>0){
                    rb.velocity= new Vector2(rb.velocity.x-fricDeAccel,rb.velocity.y);
                }else if(rb.velocity.x<0){
                    rb.velocity= new Vector2(rb.velocity.x+fricDeAccel,rb.velocity.y);
                }
                
            }else{
                if(rb.velocity.x>0){
                    rb.velocity= new Vector2(rb.velocity.x-airDeAccel,rb.velocity.y);
                }else if(rb.velocity.x<0){
                    rb.velocity= new Vector2(rb.velocity.x+airDeAccel,rb.velocity.y);
                }

            }

            if(Input.GetAxisRaw("Walk")>0){
                walking=true;
            }else{
                walking=false;
            }
            //Debug.Log(Input.GetAxisRaw("Crouch")+" "+crouched);
            if(Input.GetAxisRaw("Crouch")!=0&&!crouched){
                crouched = true;
                Coll.size=crouchCollSize;
                Coll.offset=crouchCollOff;
                anim.SetBool("isCrouched",true);
                if(!grounded){
                    jumpFrames=maxJumpFrames;
                }
                //crouchColl.enabled=true;
               // defColl.enabled=false;
            }else if(Input.GetAxisRaw("Crouch")==0&&CanStand()&&crouched){
                crouched=false;
                anim.SetBool("isCrouched",false);
                Coll.size= defCollSize;
                Coll.offset=defCollOff;
                jumpFrames=maxJumpFrames;
                // crouchColl.enabled=false;
                // defColl.enabled=true;
            }

            if(walking&&grounded){
                anim.SetFloat("AniSpeed",0.6f);
                //rb.velocity=new Vector2(Math.Max(-maxVelW,Math.Min(maxVelW,rb.velocity.x)),rb.velocity.y);
                //scan for crumble and shake... need shake once have sprites
                rb.velocity=new Vector2(rb.velocity.x/velDeAccelW,rb.velocity.y);
            }else if(crouched&&grounded){
                rb.velocity=new Vector2(rb.velocity.x/velDeAccelC,rb.velocity.y);
                //rb.velocity=new Vector2(Math.Max(-maxVelC,Math.Min(maxVelC,rb.velocity.x)),rb.velocity.y);
                //scan for crumble and shake... need shake once have sprites
            }else if(rb.velocity.x!=0){
                anim.SetFloat("AniSpeed",1f);
                //List<Collider2D> coll = new List<Collider2D>();
                ContactFilter2D filter = new ContactFilter2D();
                filter.SetLayerMask(crumbleLayer);
                List<Collider2D> coll= CheckGrounded(new List<Collider2D>(),filter);
                
                coll.ForEach(delegate(Collider2D cColl){
                    //Debug.Log(cColl);
                    //cColl.enabled=false;
                    //sprite animation for destroy
                } );
                rb.velocity=new Vector2(rb.velocity.x/velDeAccelS,rb.velocity.y);
                //rb.velocity=new Vector2(Math.Max(-maxVelS,Math.Min(maxVelS,rb.velocity.x)),rb.velocity.y);
            }

        }
    }
        int i=0;
        foreach(Key cKey in keys){
            i++;
            cKey.setTargetLoc(-cKey.getTransform().position.x+transform.position.x-(i*1.5f+cKey.dist)*dir,-cKey.getTransform().position.y+transform.position.y+(float) Math.Sin((Time.frameCount+cKey.randomFradd)/cKey.bounceSpeed)/2);
        }
       // Debug.Log(Player.Instance.grounded+" "+Player.Instance.jumpPress+" "+jumpFrames);
    }

    public void newLevel(){
        freeze=100;
        //transform.position=Vector2.zero;
        teleport=Vector2.zero;
        rb.velocity=Vector2.zero;
    }
    bool Dash(){
        anim.SetBool("Dashing", true);
        rb.velocity=new Vector2(dashVel*dir,0);
        rb.gravityScale=0;

        Coll.size= dashCollSize;
        Coll.offset=dashCollOff;


        dashFrameCount++;
        if(dashFrameCount==dashFrames){
            dashFrameCount=0;
            return true;
        }
        return false;
    }
    bool CheckGrounded(){
        bool toReturn = false;
        //Debug.Log(groundCheckCont.transform.childCount);
        for(int i=0; i<groundCheckCont.transform.childCount; i++){
            if(Physics2D.OverlapCircle(groundCheckCont.transform.GetChild(i).transform.position, groundCheckRad, groundLayer)|Physics2D.OverlapCircle(groundCheckCont.transform.GetChild(i).transform.position, crumbleCheckRad, crumbleLayer)){
                toReturn=true;
            }
        }
        //Debug.Log(toReturn);
        return toReturn;
    }
     List<Collider2D> CheckGrounded(List<Collider2D> coll, ContactFilter2D filter){

        for(int i=0; i<groundCheckCont.transform.childCount; i++){
           Physics2D.OverlapCircle(groundCheckCont.transform.GetChild(i).transform.position,crumbleCheckRad,filter,coll);
        }
        return coll;
    }
    bool CanStand()
    {        
        // LayerMask mask = LayerMask.GetMask("Ground");
        // LayerMask mask2 = LayerMask.GetMask("Crumble");
    RaycastHit2D GroundHit = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, groundLayer);
    RaycastHit2D CrumbleHit = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, crumbleLayer);
    
    if (GroundHit.collider != null)
    {
        // Check the distance to make sure the character has clearance, you'll have to change the 1.0f to what makes sense in your situation.
        if (GroundHit.distance <= defCollSize.y-crouchCollSize.y)
        {
            return false;
        }
    }

    if (CrumbleHit.collider != null)
    {
        
        // Check the distance to make sure the character has clearance, you'll have to change the 1.0f to what makes sense in your situation.
        if (CrumbleHit.distance <= defCollSize.y-crouchCollSize.y)
        {
            return false;
        }
    }
    
    return true;    
    }

    public void giveKey(Key toCollect){
        keys.Add(toCollect);
    }

    public void refill(Spilk toSpilk){
        jumps=jumpRefillNum;
        dashes=dashRefillNum;
        wallStam=wallMaxStam;
        spilks.Add(toSpilk);
    }

    public void DIE(Vector2 respawnPoint){
        
        if(!dead){
        rb.gravityScale=9;
        dashPress=false;
        dashes--;   
        anim.SetBool("Dashing", false);

        crouched = true;
        Coll.size=crouchCollSize;
        Coll.offset=crouchCollOff;

        freeze=25;
        LevelControl.Instance.black=true;
        LevelControl.Instance.anim.SetFloat("Speed", 2f);
        if(respawnPoint!=null){
            //transform.position=respawnPoint;
            teleport=respawnPoint;
            rb.velocity = Vector2.zero;
        }
        foreach (Key cKey in keys)
        {
            cKey.Reset();
        }
        keys= new List<Key>();
        }
        dead=true;
        //reset level items
    }

    public void ignoreCollision(Collider2D other, bool toSet=true){
        Physics2D.IgnoreCollision(Coll,other,toSet);
        //Physics2D.IgnoreCollision(crouchColl,other,toSet);
    }




    
}

