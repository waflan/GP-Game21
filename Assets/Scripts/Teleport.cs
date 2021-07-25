using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform tpTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider collision){
        if(collision.GetComponent<Rigidbody>()){
            collision.transform.position=tpTarget.rotation*(collision.transform.position-this.transform.position)+tpTarget.position;
            // if(collision.GetComponent<PlayerControl>()){
            //     collision.GetComponent<PlayerControl>().playerVectorUp=tpTarget.rotation*collision.GetComponent<PlayerControl>().playerVectorUp;
            // }
        }
    }
}
