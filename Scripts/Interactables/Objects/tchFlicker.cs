using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class tchFlicker : MonoBehaviour
{
    [SerializeField] private Vector3 rRange,gRange,bRange,radRange,fRange,iRange;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D light;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float roll = UnityEngine.Random.Range(0,100);
        (float, float, float) nextColor = (light.color.r,light.color.g,light.color.b);

        if(rRange.z>roll){
            nextColor.Item1=UnityEngine.Random.Range(rRange.x,rRange.y);
        }if(gRange.z>roll){
            nextColor.Item2=UnityEngine.Random.Range(gRange.x,gRange.y);
        }if(bRange.z>roll){
            nextColor.Item3=UnityEngine.Random.Range(bRange.x,bRange.y);
        }if(radRange.z>roll){
            light.pointLightOuterRadius =UnityEngine.Random.Range(radRange.x,radRange.y);
        }if(fRange.z>roll){
            light.falloffIntensity=UnityEngine.Random.Range(fRange.x,fRange.y);
        }if(iRange.z>roll){
            light.intensity=UnityEngine.Random.Range(iRange.x,iRange.y);
        }

        light.color = new Color(nextColor.Item1,nextColor.Item2,nextColor.Item3);  
        
    }
}
