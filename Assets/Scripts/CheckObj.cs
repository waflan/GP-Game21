using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CheckObj : MonoBehaviour
{
    public Vector3 camPos=new Vector3();
    public Quaternion camRot=new Quaternion();
    public Transform child;
    bool able2Check,isCheck,befCheck;
    public GameObject guideObj;

    public UnityEvent onCheck;
    public UnityEvent onCheckDisable;
    public bool searchPlayer=false;
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
        guideObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isCheck&&!befCheck){
            onCheck.Invoke();
        }else if(!isCheck&&befCheck){
            onCheckDisable.Invoke();
        }
        befCheck=isCheck;
    }

    private void OnTriggerStay(Collider collision)
    {
        if(collision.tag=="Player"){
            if(collision.GetComponent<PlayerControl>()){
                PlayerControl pctrl = collision.GetComponent<PlayerControl>();
                pctrl.able2Check=true;
                able2Check=true;
                if(guideObj!=null){
                    guideObj.SetActive(able2Check&&!pctrl.isCheck);
                }
                isCheck=pctrl.isCheck;
            }
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if(collision.tag=="Player"){
            if(collision.GetComponent<PlayerControl>()){
                PlayerControl pctrl = collision.GetComponent<PlayerControl>();
                pctrl.able2Check=false;
                able2Check=false;
                if(guideObj!=null){
                    guideObj.SetActive(able2Check&&!pctrl.isCheck);
                }
                isCheck=pctrl.isCheck;
            }
        }
    }
}
