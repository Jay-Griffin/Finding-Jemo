using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Camera : MonoBehaviour
{

    public static Camera Instance {get; private set;}

    void Awake() {

        if(Instance!=null && Instance!=this){
            Destroy(this);
        }else{
            Instance=this;
            //Debug.Log(Instance);
        }
    }
    // Start is called before the first frame update
    [SerializeField] float dist, zoom,zoomRate;
    [SerializeField] private GameObject player;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(Input.GetAxisRaw("Zoom"));
        zoom+=Input.GetAxisRaw("Zoom")/-zoomRate;
        zoom=Math.Max(4f,Math.Min(4f,zoom));
        //GetComponent<UnityEngine.PixelPerfectCamera>().orthographicSize=zoom;
        GetComponent<UnityEngine.Experimental.Rendering.Universal.PixelPerfectCamera>().refResolutionX = (int)   (160*zoom);
        GetComponent<UnityEngine.Experimental.Rendering.Universal.PixelPerfectCamera>().refResolutionY = (int)   (90*zoom);
        //transform.position = new Vector3(Player2.Instance.transform.position.x,Player2.Instance.transform.position.y,-10);
       // Debug.Log(player);
        transform.position = new Vector3(transform.position.x+(-transform.position.x+Player2.Instance.gameObject.transform.position.x)/dist,transform.position.y+(-transform.position.y+Player2.Instance.gameObject.transform.position.y)/dist,-10);
       // transform.position = new Vector3(player.transform.position.x,player.transform.position.y,-10);
    }

    public void textZoom(float tz){

        zoom+=Input.GetAxisRaw("Zoom")/-zoomRate;
        zoom=tz;
        //GetComponent<UnityEngine.PixelPerfectCamera>().orthographicSize=zoom;
        GetComponent<UnityEngine.Experimental.Rendering.Universal.PixelPerfectCamera>().refResolutionX = (int)   (160*zoom);
        GetComponent<UnityEngine.Experimental.Rendering.Universal.PixelPerfectCamera>().refResolutionY = (int)   (90*zoom);
        //transform.position = new Vector3(Player.Instance.x,Player.Instance.y,-10);
        transform.position = new Vector3(transform.position.x+(-transform.position.x+Player2.Instance.transform.position.x)/dist,transform.position.y+(-transform.position.y+Player2.Instance.transform.position.y)/dist,-10);
    }
}
