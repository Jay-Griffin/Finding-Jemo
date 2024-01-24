using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeDialogue : MonoBehaviour
{
    [SerializeField] private string Dialogue;
    string[] dialogue1;
    bool hasDiscussed;
    // Start is called before the first frame update
    void Start()
    {
        hasDiscussed=false;
        Dialogue=Dialogue.Replace("/n","\n");
        Dialogue=Dialogue.Replace("\\n","\n");
        dialogue1=Dialogue.Split("`");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")&!hasDiscussed){
            hasDiscussed=true;
            Player2.Instance.Dialogue=dialogue1;
            Player2.Instance.dispDial=true;
            
        }
    }
}
