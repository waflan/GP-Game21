using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimeEventController : MonoBehaviour
{
    public Animator animator;
    public Vector3 offset=new Vector3();
    void Start()
    {
        animator=GetComponent<Animator>();
        transform.localPosition=offset;
        transform.localRotation=Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        if(animator!=null){
            if(!animator.GetCurrentAnimatorStateInfo(0).IsName("SquatWalk")){
                transform.localPosition=offset;
                transform.localRotation=Quaternion.identity;
            }
        }else{
            transform.localPosition=offset;
            transform.localRotation=Quaternion.identity;
        }
        
    }
    void OnCallChangeFace(){}
}
