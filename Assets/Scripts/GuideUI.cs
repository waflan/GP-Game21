using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideUI : MonoBehaviour
{
    public Transform camTransform;

    public Transform rotateTransform;
    public float hideDistance=20;
    // Start is called before the first frame update
    void Start()
    {
        if(camTransform==null){
            camTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
        if(rotateTransform==null){
            if(transform.childCount!=0){
                rotateTransform = this.transform.GetChild(0);
            }else{
                rotateTransform=this.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if((rotateTransform.position-camTransform.position).sqrMagnitude>=hideDistance*hideDistance){
            rotateTransform.gameObject.SetActive(false);
        }else{
            rotateTransform.gameObject.SetActive(true);
        }
        rotateTransform.rotation=Quaternion.LookRotation(rotateTransform.position-camTransform.position,camTransform.rotation*Vector3.up);
    }
}
