using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAnimation1 : MonoBehaviour
{
	public bool enableTransformUpdate=true;
    Rigidbody rig;
    public Vector3 center=new Vector3(0,0,0);
	public Vector3 up = Vector3.up;
	public float radius=5;
    public float rotateSpeed=10;
	float nowRot=0;

    // Start is called before the first frame update
    void Start()
    {
        rig=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		nowRot=Mathf.Repeat(nowRot+rotateSpeed*Time.fixedDeltaTime,360);
		if(enableTransformUpdate){
			transform.position = center+Quaternion.AngleAxis(nowRot,up)*Vector3.forward*radius;
			transform.rotation = Quaternion.AngleAxis(nowRot+90,up);
		}
		
		rig.velocity = Quaternion.AngleAxis(nowRot,up)*Vector3.right*radius;
		rig.angularVelocity = Quaternion.FromToRotation(Vector3.up,up)*new Vector3(0,Mathf.Deg2Rad*rotateSpeed,0);
    }
}
