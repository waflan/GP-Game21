using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSquatTrigger : MonoBehaviour
{
    // public bool isOn{get;private set;}
    public bool isOn;
    bool nextState;
    void FixedUpdate(){
        nextState=false;
		StartCoroutine(AfterFixedUpdateCoroutine());
    }
        IEnumerator AfterFixedUpdateCoroutine(){
		yield return new WaitForFixedUpdate();
		AfterFixedUpdate();
	}
	void AfterFixedUpdate(){
        isOn=nextState;
    }

    private void OnTriggerStay(Collider collision){
        if(!nextState&&!collision.isTrigger){
            if(!collision.GetComponentInParent<PlayerControl>()){
                nextState=true;
            }
        }
    }
}
