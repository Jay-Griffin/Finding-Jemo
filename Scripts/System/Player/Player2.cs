using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player2 : MonoBehaviour
{
    //-------------------Movement stuff
    [SerializeField] private float walkSpeed, runSpeed, crouchSpeed,airEffector, jump, jumpKill;
    float moveSpeed;
    bool emergshort;
    Rigidbody2D rb;
    public int dir{get; private set;}

    //------------------Collider Variables
    BoxCollider2D Coll;
    Vector2 defCollOff, defCollSize, crouchCollOff, crouchCollSize, dashCollOff, dashCollSize;

    //------------------Timer Variables
    float cHangTime, cJumpBuffer, freezeCount;
    [SerializeField] private float maxHangTime, maxJumpBuffer;

    //------------------Grounded Variables
    bool grounded;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private float castDistance, castOffX;
    [SerializeField] private LayerMask ground, crumble,oneWay;

    //------------------------animator things
    Animator anim;
    [SerializeField] private GameObject sprite;

    //---------------------------Death-ish
    public bool dead;
    public Vector2? teleport;

    //---------------------------Dialogue
    [SerializeField] private GameObject DialBox;
    [SerializeField] private TMP_Text dialText;
    public bool dispDial{get; set;}
    public string[] Dialogue{get; set;}
    int textIndex;

    //---------------------------Keys/Collectables
    public List<Key> keys {get; private set;}

    //--------------------------Dash
    [SerializeField] private float dashVel, dashLength;
    float cDash;
    [SerializeField] private int dashRefill;
    int dashes;
    public bool dashPress{get; private set;}
    public bool canDash{get; set;}

    //--------------------------WallJump
    bool isWalled;
    [SerializeField] private float offWallVel, wallJumpLength;
    [SerializeField] private Vector2 upperWallOffset, lowerWallOffset, boxSizeWall;
    public bool canWall{get; set;}
    [SerializeField] private float maxWallHang, maxWallStam, wallFallFrac;
    float cWallHang, cWallStam;

    //--------------------------Spilk/Refill
    bool isSpilked;

    //--------------------------Icons
    [SerializeField] private GameObject DashIc;
    [SerializeField] private GameObject SpilkIc;



    public static Player2 Instance {get; private set;}
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        Coll= GetComponent<BoxCollider2D>();
        if(Instance!=null && Instance!=this){
            Destroy(this);
        }else{
            Instance=this;
        }
        keys = new List<Key>();
    }

    void Start()
    {
        defCollOff = new Vector2(0.00f,-0.24f);
        defCollSize = new Vector2(0.75f,2.54f);

        crouchCollOff = new Vector2(0.00f, -0.77f);
        crouchCollSize = new Vector2(0.75f, 1.47f);

        dashCollOff = new Vector2(0.00f,-0.24f);
        dashCollSize = new Vector2(0.75f,2.0f);

        moveSpeed = runSpeed;

        anim = sprite.GetComponent<Animator>();

        freezeCount = 0;

        dead = false;

        DialBox.SetActive(false);
        textIndex=0;
        dispDial=false;
        
        dashes=dashRefill;

        canDash=false;
        canWall=false;
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.gravityScale=9;
        anim.SetFloat("Speed",Mathf.Abs(rb.velocity.x));
        
        updateCounters(Time.deltaTime);
        grounded = getGrounded();
        anim.SetBool("Grounded",grounded);
        isWalled= getWalled();
        anim.SetBool("Walled", isWalled);
        
        upKeyPos();
        dialogue();
    
        if(dashes>0&canDash){
            DashIc.SetActive(true);
        }else{
            DashIc.SetActive(false);
        }
        SpilkIc.SetActive(isSpilked);
       // Debug.Log("X: "+rb.velocity.x+" Y: "+rb.velocity.y);
        if(Input.GetButtonDown("Jump")){
            cJumpBuffer=0;
        }
        if(freezeCount<0){
            if(Input.GetButtonDown("Dash")&dashes>0){
                cDash=dashLength;
                dashes--;
            }
            dead=false;
            if(teleport!=null){
                transform.position=(Vector3) teleport;
                rb.velocity = Vector2.zero;
                teleport=null;
            }
            if(Input.GetAxisRaw("Horizontal")>0){
                dir=1;
                transform.rotation = Quaternion.Euler(0,0,0);
            }else if(Input.GetAxisRaw("Horizontal")<0){
                dir=-1;
                transform.rotation = Quaternion.Euler(0,180,0);
            }
            
            //--------------Jump Logic
            if(grounded){
                cHangTime=0;
                isSpilked=false;
            }
            if(isWalled){
                cWallHang=0;
                isSpilked=false;
            }
            
            if(cJumpBuffer<maxJumpBuffer&((cHangTime<maxHangTime)|isSpilked)){
                isSpilked=false;
                if(!(Input.GetAxisRaw("Crouch")>0)){
                    rb.velocity = new Vector2(rb.velocity.x,jump);
                }else{
                    rb.velocity = new Vector2(rb.velocity.x*dir,jump/1.5f);
                }
                cJumpBuffer=maxJumpBuffer;
                anim.SetBool("JumpPress",true);
                if(emergshort){
                    rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y*jumpKill);
                    //anim.SetBool("JumpPress",false);
                }
            }
            if((Input.GetButtonUp("Jump")|Input.GetButtonDown("Crouch"))&&rb.velocity.y>0){
                rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y*jumpKill);
            //anim.SetBool("JumpPress",false);
            }
            if(rb.velocity.y<=0){
                anim.SetBool("JumpPress",false);
            }
            if(Input.GetButtonUp("Jump")&&cJumpBuffer<maxJumpBuffer){
                    emergshort=true;
            }
            if(grounded){
                emergshort=false;
            }

            if(cJumpBuffer<maxJumpBuffer&!grounded&canWall&(cWallHang<maxWallHang)){
                if(isWalled){
                   // Debug.Log("On wall");
                    rb.velocity=new Vector2(offWallVel*-dir,jump);
                }else{
                    rb.velocity=new Vector2(offWallVel*dir,jump);
                   // Debug.Log("Off wall");
                }
               // Debug.Log(rb.velocity.x);
                cJumpBuffer=maxJumpBuffer;
                freezeCount=wallJumpLength;
                //Debug.Log("WALLJUMP");
            }else if(isWalled&!grounded&canWall&Input.GetAxisRaw("Horizontal")==dir){
                rb.velocity=new Vector2(rb.velocity.x,0);
                rb.gravityScale=0;
            }else if(isWalled&!grounded&canWall&rb.velocity.y<0){
                rb.velocity=new Vector2(rb.velocity.x, rb.velocity.y*(wallFallFrac));
                
            }
            


            if(Input.GetAxisRaw("Crouch")>0){
                anim.SetFloat("AniSpeed",1f);
                moveSpeed=crouchSpeed;
                Coll.size=crouchCollSize;
                Coll.offset=crouchCollOff;
                anim.SetBool("isCrouched",true);
            }else if(Input.GetAxisRaw("Walk")>0&CanStand()&getGrounded()){
                Coll.size= defCollSize;
                Coll.offset=defCollOff;
                moveSpeed=walkSpeed;
                anim.SetFloat("AniSpeed",0.6f);

            }else if(CanStand()){
                moveSpeed=runSpeed;
                anim.SetFloat("AniSpeed",1f);
                Coll.size= defCollSize;
                Coll.offset=defCollOff;
                anim.SetBool("isCrouched",false);
            }


        }else{
            //rb.velocity=Vector2.zero;
        }
        Dash();

    }

    private void FixedUpdate() {
        if(freezeCount<0){
            if(grounded){
                rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal")*moveSpeed,rb.velocity.y);
            }else if(!grounded){
                if(Input.GetAxisRaw("Horizontal")==0&rb.velocity.x!=0){
                    rb.velocity = new Vector2(Mathf.Max(-runSpeed,Mathf.Min(runSpeed,rb.velocity.x-Mathf.Abs(rb.velocity.x)/rb.velocity.x*airEffector/2)),rb.velocity.y);
                }else{
                    rb.velocity = new Vector2(Mathf.Max(-runSpeed,Mathf.Min(runSpeed,rb.velocity.x+(airEffector*dir))),rb.velocity.y);
                }
                //rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal")*runSpeed,rb.velocity.y);
            }
        }
    }


    void updateCounters(float dt){
        cHangTime+=dt;
        cJumpBuffer+=dt;
        freezeCount-=dt;
        cWallHang+=dt;
    }
    public bool getGrounded(){
        return Physics2D.BoxCast(transform.position+Vector3.right*castOffX, boxSize, 0, -transform.up, castDistance, ground)| Physics2D.BoxCast(transform.position+Vector3.right*castOffX, boxSize, 0, -transform.up, castDistance, crumble)|Physics2D.BoxCast(transform.position+Vector3.right*castOffX, boxSize, 0, -transform.up, castDistance, oneWay);
    }
    private void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position-transform.up*castDistance+Vector3.right*castOffX, boxSize);
        /*
        Gizmos.DrawWireCube(transform.position-transform.up*castDistanceWall+Vector3.right*castOffXWall*dir, boxSizeWall);
        Gizmos.DrawWireCube(transform.position-transform.up*castDistanceWall2+Vector3.right*castOffXWall2*dir, boxSizeWall);
        */
        Gizmos.DrawWireCube(transform.position+Vector3.right*upperWallOffset.x*dir+Vector3.up*upperWallOffset.y,boxSizeWall);
        Gizmos.DrawWireCube(transform.position+Vector3.right*lowerWallOffset.x*dir+Vector3.up*lowerWallOffset.y,boxSizeWall);

    }
    public void newLevel(){
        freezeCount=1.5f;
        rb.velocity = Vector2.zero;
        teleport = Vector2.zero+Vector2.up;

        canDash=false;
        canWall=false;

        foreach (Key cKey in keys){
                cKey.Reset();
            }
            keys= new List<Key>();
    }
    public void DIE(Vector2 rp){
         
        if(!dead){
            //Reset posture
            //crouched = true;
            Coll.size=crouchCollSize;
            Coll.offset=crouchCollOff;

            //Black screen
            freezeCount=0.5f;
            LevelControl.Instance.black=true;
            LevelControl.Instance.anim.SetFloat("Speed", 2f);

            //Send to Respawn Point
            if(rp!=null){
                teleport=rp;
                rb.velocity = Vector2.zero;
            }

            //Reset keys
            foreach (Key cKey in keys){
                cKey.Reset();
            }
            keys= new List<Key>();

            //Reset perms
            canDash=false;
            canWall=false;
            
        }
        dead=true;
        anim.SetBool("JumpPress", false);
    }
    bool CanStand(){        
        RaycastHit2D GroundHit = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, ground);
        RaycastHit2D CrumbleHit = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, crumble);
        
        if (GroundHit.collider != null){
            if (GroundHit.distance <= defCollSize.y-crouchCollSize.y){
                return false;
            }
        }
        if (CrumbleHit.collider != null)
        {
            if (CrumbleHit.distance <= defCollSize.y-crouchCollSize.y)
            {
                return false;
            }
        }
        
        return true;    
    }
    public void giveKey(Key toGive){
        keys.Add(toGive);
    }
    public void ignoreCollision(Collider2D other, bool toSet=true){
        Physics2D.IgnoreCollision(Coll,other,toSet);
    }
    void upKeyPos(){
        int i=0;
        foreach(Key cKey in keys){
            i++;
            cKey.setTargetLoc(-cKey.getTransform().position.x+transform.position.x-(i*1.5f+cKey.dist)*dir,-cKey.getTransform().position.y+transform.position.y+(float) Mathf.Sin((Time.frameCount+cKey.randomFradd)/cKey.bounceSpeed)/2);
        }
    }
    void Dash(){
        if(cDash>0&canDash){
            freezeCount=0.05f;
            rb.velocity=new Vector2(dir*dashVel,0);
            rb.gravityScale=0;
            dashPress=true;
            Coll.offset=dashCollOff;
            Coll.size=dashCollSize;
            anim.SetBool("Dashing",true);
        }else{
            dashPress=false;
            anim.SetBool("Dashing",false);
            if(grounded|(isWalled&canWall)){
                dashes=dashRefill;
            }
        }
        cDash-=Time.deltaTime;
    }
    void dialogue(){
        if(dispDial){
            freezeCount=.25f;
            rb.velocity=Vector2.zero;
            DialBox.SetActive(true);
            dialText.text=Dialogue[textIndex];
            if(Input.GetKeyDown("return")){
                textIndex++;
                if(textIndex>=Dialogue.Length){
                    dispDial=false;
                }
            }
        }else{
            DialBox.SetActive(false);
            textIndex=0;
        }
    }
    public bool getWalled(){
        return (
            (
                Physics2D.BoxCast(transform.position+Vector3.right*upperWallOffset.x*dir+Vector3.up*upperWallOffset.y, boxSizeWall, 0, -transform.up, .01f, ground)
                | Physics2D.BoxCast(transform.position+Vector3.right*upperWallOffset.x*dir+Vector3.up*upperWallOffset.y, boxSizeWall, 0, -transform.up, .01f, crumble)
           )&(
               Physics2D.BoxCast(transform.position+Vector3.right*lowerWallOffset.x*dir+Vector3.up*lowerWallOffset.y, boxSizeWall, 0, -transform.up, .01f, ground)
                | Physics2D.BoxCast(transform.position+Vector3.right*lowerWallOffset.x*dir+Vector3.up*lowerWallOffset.y, boxSizeWall, 0, -transform.up, .01f, crumble)
            )
            )&canWall;
            
 
    }

    public void refill(Spilk spl){
        isSpilked=true;
        dashes=dashRefill;
        //add spilk to a collection to respawn on death


    }
}
  
