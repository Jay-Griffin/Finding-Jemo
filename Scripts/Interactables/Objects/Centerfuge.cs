using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centerfuge : MonoBehaviour
{
 
    //[SerializeField] private float parralaxLayer;
    [SerializeField] private  float turnRate;
    [SerializeField] private bool lockY, defEn;
    private Player player;
    private Vector2 lastLoc;


    // Start is called before the first frame update
    void Start()
    {
        lastLoc = new Vector2(0,0);
        player = Player.Instance;
        this.gameObject.SetActive(defEn);
        //parralaxLayer = 100f;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 diff = lastLoc - (Vector2) player.transform.position;

        int lockYInt = lockY ? 0 : 1;

        //transform.position-= Vector3.right*((Vector3) diff*(parralaxLayer/100)).x+Vector3.up*((Vector3) diff*(parralaxLayer/100)).y*lockYInt;
        transform.position = new Vector3(transform.position.x+(-transform.position.x+Player.Instance.x)/4,transform.position.y,transform.position.z);

        transform.Rotate(0,diff.x*turnRate,0);
        lastLoc= player.transform.position;



        
    }
}
