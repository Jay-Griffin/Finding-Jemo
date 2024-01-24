using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParralaxedObj : MonoBehaviour
{
    [SerializeField] private float parralaxLayer;
    [SerializeField] private bool lockY, defEn;
    private Player player;
    private Vector2 lastLoc;


    // Start is called before the first frame update
    void Start()
    {
        lastLoc = new Vector2(0,0);
        player = Player.Instance;
        this.gameObject.SetActive(defEn);

        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 diff = lastLoc - (Vector2) player.transform.position;

        int lockYInt = lockY ? 0 : 1;

        transform.position-= Vector3.right*((Vector3) diff*(parralaxLayer/100)).x+Vector3.up*((Vector3) diff*(parralaxLayer/100)).y*lockYInt;
        lastLoc= player.transform.position;

        
    }

    
}
