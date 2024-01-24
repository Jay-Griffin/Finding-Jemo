using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public string colorReqs;

    public int toLevel;

    string[] colorReqIds;
    // Start is called before the first frame update
    void Start()
    {
        colorReqIds = colorReqs.Split(",");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    bool canUnlock(List<Key> playerKeys){
        if(colorReqIds.Length==1){
            if(colorReqIds[0]=="0"){
                return true;
            }
        }
        //copy players keylist as String form IDs
        List<int> playerKeysIDCopy = new List<int>();
        playerKeys.ForEach((item)=>{
            playerKeysIDCopy.Add(item.color);
        });
        
        //loop through colorReqId's
        int removedKeys=0;
        for(int i=0; i<colorReqIds.Length;i++){
            for(int j=0; j<playerKeysIDCopy.Count; j++){

                //remove key from player key list on matches
                if(colorReqIds[i]==playerKeysIDCopy[j]+""){
                    removedKeys++;
                    playerKeysIDCopy.Remove(playerKeysIDCopy[j]);
                }
            }
        }
        Debug.Log(removedKeys);
        
         if(removedKeys==colorReqIds.Length){
            for(int i=0; i<colorReqIds.Length;i++){
                for(int j=0; j<playerKeys.Count; j++){
                    if(colorReqIds[i]==playerKeys[j].color+""){
                        playerKeys[j].Reset();
                        playerKeys.Remove(playerKeys[j]);
                    }
                }
            }
            return true;
         }
         return false;
    }
        void OnTriggerEnter2D(Collider2D other)
    {

        if(other.CompareTag("Player")){
            if(canUnlock(Player2.Instance.keys)){
                Debug.Log(toLevel);
                LevelControl.Instance.changeLevel(toLevel);
            }
        }
    }
}
