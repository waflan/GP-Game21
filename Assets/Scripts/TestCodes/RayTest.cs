using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
