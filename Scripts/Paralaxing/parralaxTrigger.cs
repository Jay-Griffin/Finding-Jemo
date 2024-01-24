using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parralaxTrigger : MonoBehaviour
{
    [SerializeField] private bool defEn;

    [SerializeField] private GameObject parralaxObj;

    //BoxCollider2D bc;
    // Start is called before the first frame update
    void Start()
    {
        //bc=getComponent<BoxCollider2D>();
        //parralaxObj.SetActive(defEn);
    }

    // Update is called once per frame
    void Update()
    {
        //parralaxObj.SetActive(defEn);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            parralaxObj.SetActive(defEn);
        }
    }

}
