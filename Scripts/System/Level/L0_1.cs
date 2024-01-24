using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L0_1 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject Sun;
    [SerializeField] private GameObject Bkg;

    [SerializeField] private float upTween, maxUp;

    SpriteRenderer spr;
    void Start()
    {
        spr=Bkg.GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Sun.transform.position+=Vector3.up*upTween*(1-(Sun.transform.position.y/maxUp));
        //spr.color=new Color(spr.color.r+0.01f,spr.color.g+0.01f,spr.color.b);
    }
}
