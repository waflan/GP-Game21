using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTest : MonoBehaviour
{
    Rigidbody rig;
    public Vector3 angularVelocity,inertiaTensor=new Vector3(114514,0.0001f,114514);
    // Start is called before the first frame update
    void Start()
    {
        rig=GetComponent<Rigidbody>();
    }

    void FixedUpdate(){
        rig.velocity=Vector3.zero;
        // Debug.Log(rig.inertiaTensor);
        rig.inertiaTensor=inertiaTensor;
        rig.angularVelocity=angularVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position,transform.rotation*Vector3.forward);
        if(Physics.Raycast(ray,out hit)){
            Debug.DrawLine(hit.point,hit.point+hit.normal);
            Debug.DrawLine(transform.position,hit.point);
        }else{
            Debug.DrawRay(transform.position,transform.rotation*Vector3.forward);
        }
        
    }

}
