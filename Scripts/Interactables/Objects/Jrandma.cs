using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jrandma : MonoBehaviour
{
    int timesTalked;
    string[] dialogue1;
    // Start is called before the first frame update
    void Start()
    {
        dialogue1 = new string[]{"Oh! I didnt see you there sweaty...\nWelcome to the art museum!\n(Featuring the same 4 peices of art over and over again)","This one here is my favorite.\nPlease stay to enjoy the art!", "Oh you need to get through?\nWhat a shame that is...", "Well if you insist on heading through you're going to have to climb going forward,\nlet me take a look at your hands real quick","Hmmm...","...","...\nReally moist\n...","...","No these wont work...\nHere just take mine!","Dont worry Im old\nI wont need them soon anyways","Using these will help you grab onto,\nand jump off walls with ease!","Grandma Hands\nYou can now grab onto nearby walls and hang there by moving into them.\nAdditionally you can now jump off walls and walls will recharge your dash."};
        timesTalked=0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            if(timesTalked<1){
                transform.rotation=Quaternion.Euler(0,180,0);
                Player2.Instance.Dialogue=dialogue1;
                Player2.Instance.dispDial=true;
                timesTalked++;
            }
        }
    }
    /// <summary>
    /// Sent when a collider on another object stops touching this
    /// object's collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            if(timesTalked<1){
                transform.rotation=Quaternion.Euler(0,0,180);
            }
        }
    }
}
