using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAnimation1 : MonoBehaviour
{
    Rigidbody rig;

    // Start is called before the first frame update
    void Start()
    {
        rig=GetComponent<Rigidbody>();
        rig.velocity=new Vector3(5,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vel = this.transform.position.normalized;
        vel.y=0;
        rig.velocity=Quaternion.AngleAxis(90,Vector3.up)*vel;
        rig.angularVelocity=new Vector3(0,Mathf.Deg2Rad*10,0);
    }
}
