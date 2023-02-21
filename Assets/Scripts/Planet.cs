using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public Vector3 defaultUp=Vector2.up;
    float gravityPower=Physics.gravity.magnitude;
    public float gravityPowerScale = 1;
    public float radius=1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.GetComponent<Rigidbody>()){
            // Debug.Log("Enter");
            Rigidbody objRig = collision.GetComponent<Rigidbody>();
            objRig.useGravity=false;
            if(collision.GetComponent<PlayerControl>()){
                PlayerControl pctrl=collision.GetComponent<PlayerControl>();
                pctrl.isChangedGField=true;
                // Debug.Log("Enter Player");
            }
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        if(collision.GetComponent<Rigidbody>()){
            Rigidbody objRig = collision.GetComponent<Rigidbody>();
            objRig.useGravity=false;
            Vector3 toCenter = (collision.transform.position-this.transform.position).normalized;
            objRig.AddForce(-toCenter*gravityPower,ForceMode.Acceleration);
            if(collision.GetComponent<PlayerControl>()){
                PlayerControl pctrl=collision.GetComponent<PlayerControl>();
                pctrl.radiusUpVector=radius;
                pctrl.playerUpVector=toCenter;
				// this.transform.position-=playerVectorUp*(rig.velocity.magnitude*Mathf.Sin(changeAngle)*0.05f);
            }
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if(collision.GetComponent<Rigidbody>()){
            Rigidbody objRig = collision.GetComponent<Rigidbody>();
            objRig.useGravity=true;
            if(collision.GetComponent<PlayerControl>()){
                PlayerControl pctrl=collision.GetComponent<PlayerControl>();
                pctrl.playerUpVector=defaultUp;
                pctrl.isChangedGField=true;
            }
        }
    }
}
