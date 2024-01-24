using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jwarf : MonoBehaviour
{
    // Start is called before the first frame update
    int timesTalked;
    string[] dialogue1;
    [SerializeField] private Animator ani;
    void Start()
    {
        dialogue1 = new string[]{"Hey there!\nMy names Jild,\nIm seven years old!","My its sure been quite some time\nsince of seen someone down here...", "Whats that?\nWhys a seven year old working in the mines?","Silly goose, I love working for the company,\nthey pay me SIX WHOLE DOLLARS an hour.\nAnd if I do good they might raise me to $6.05!\nThis is the best job ever! I cant wait to be fed.","Listen, I lost my magic red converse up on the ledge to the left.\nIf you can get them they're all yours!\nGood Luck!"};
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
                Player2.Instance.Dialogue=dialogue1;
                Player2.Instance.dispDial=true;
                ani.SetBool("In Dialogue",true);
                timesTalked++;
            }
        }
    }
    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        ani.SetBool("In Dialogue",false);
    }
}
