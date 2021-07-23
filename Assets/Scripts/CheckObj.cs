using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObj : MonoBehaviour
{
    public Vector3 camPos=new Vector3();
    public Quaternion camRot=new Quaternion();
    public Transform child;
    // Start is called before the first frame update
    void Start()
    {
        this.tag="CheckTarget";
        if(transform.childCount==0){
            GameObject gameObject= new GameObject("camPos");
            gameObject.transform.position=camPos;
            gameObject.transform.rotation=camRot;
            gameObject.transform.parent=transform;
            child=gameObject.transform;
        }else{
            child=transform.GetChild(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider collision)
    {
        if(collision.tag=="Player"){
            if(collision.GetComponent<PlayerControl>()){
                collision.GetComponent<PlayerControl>().able2Check=true;
            }
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if(collision.tag=="Player"){
            if(collision.GetComponent<PlayerControl>()){
                collision.GetComponent<PlayerControl>().able2Check=false;
            }
        }
    }
}
