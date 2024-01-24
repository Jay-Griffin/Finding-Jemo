using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashGetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
        Player2.Instance.canDash=true;
        Debug.Log("Dash given by: " +GetGameObjectPath(gameObject));
    }
    public static string GetGameObjectPath(GameObject obj)
    {
    string path = "/" + obj.name;
    while (obj.transform.parent != null)
    {
        obj = obj.transform.parent.gameObject;
        path = "/" + obj.name + path;
    }
    return path;
}
}
