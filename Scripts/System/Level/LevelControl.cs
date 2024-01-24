using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelControl : MonoBehaviour, IDataPers
{
    public int level;
    GameObject[] Levels;

    public UnityEngine.Rendering.Universal.Light2D GlobalLight;
    public float freeze;

    public Animator anim;
    public bool black, resLev;
    // Start is called before the first frame update
    int CompareObNames( GameObject x, GameObject y )
    {
        return x.name.CompareTo( y.name );
    }
    void Start()
    {
        Levels = GameObject.FindGameObjectsWithTag("Level");
        //GlobalLight = GameObject.FindGameObjectsWithTag("GlobalLight");
        Array.Sort( Levels, CompareObNames );
        foreach (GameObject Lev in Levels){
            Lev.SetActive(false);
        }

        freeze = 0;
       
        
    }
    public static LevelControl Instance {get; private set;}
    void Awake() {
        if(Instance!=null && Instance!=this){
            Destroy(this);
        }else{
            Instance=this;
            //Debug.Log(Instance);
        }
    }

    public void LoadData(GameData data){
        level=data.currentLevel;
        Debug.Log("Level Loaded to Controller");
        //resLev=true;
    }

    public void SaveData(ref GameData data){
 
        data.currentLevel=level;
       
    }


    private void Update() {

        anim.SetBool("black",black);
        freeze-=Time.deltaTime;

        if(freeze<0){
            if(resLev){
                changeLevel(level);
                resLev=false;
            }
         int counter = 0;
        foreach (GameObject Lev in Levels){
            if(counter == level){
                Lev.SetActive(true);
                if(Lev.name.Contains("0.")){
                    GlobalLight.intensity=0.4f;
                    GlobalLight.color = new Color(199/255f,213/255f,255/255f);
                }else if(Lev.name.Contains("1.")){
                    //Debug.Log("Here");
                    GlobalLight.intensity=0.75f;
                    GlobalLight.color = new Color(1f,.9f,.8f);
                }else if(Lev.name.Contains("2.")){
                    GlobalLight.intensity=0.625f;
                    GlobalLight.color = new Color(1f,.9f,.9f);
                    //Player.Instance.canDash=true;
                }else if(Lev.name.Contains("3.")){
                    GlobalLight.intensity=0.8f;
                    GlobalLight.color = new Color(1f,.9f,1f);
                    //Player.Instance.canDash=true;
                }
            }else{
                Lev.SetActive(false);
            }
            counter++;
        }
        }

        if(Input.GetButtonDown("Res")){
            changeLevel(0);
            // resLev=true;
        }
    }

    public int changeLevel(int newLevel){
        anim.SetBool("black",true);
        black=true;
        anim.SetFloat("Speed", .5f);
        Player2.Instance.newLevel();
        int toReturn = level;
        level = newLevel;
        freeze=1f;

        return toReturn;
    }
}
