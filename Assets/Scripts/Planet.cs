using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public Vector3 defaultUp=Vector2.up;
    float gravityPower=Physics.gravity.magnitude;
    public float gravityPowerScale = 1;
    public float radius=1;

    public int triggerCount=0;
    public Dictionary<PlayerControl,Vector2Int> playerStayCnt = new Dictionary<PlayerControl, Vector2Int>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            if(collider.isTrigger){
                triggerCount++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate(){
        List<PlayerControl> players = new List<PlayerControl>(playerStayCnt.Keys);
        foreach (PlayerControl key in players){
            Vector2Int StayCnt = playerStayCnt[key];
            StayCnt.x=0;
            playerStayCnt[key]=StayCnt;
        }
		StartCoroutine(AfterFixedUpdateCoroutine());
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.GetComponent<Rigidbody>()){
            // Debug.Log("Enter");
            Rigidbody objRig = collision.GetComponent<Rigidbody>();
            objRig.useGravity=false;

            // PlayerControl pctrl;
            // if(collision.TryGetComponent<PlayerControl>(out pctrl)){
            //     // Vector3 toCenter = (this.transform.position-collision.transform.position).normalized;
            //     // pctrl.setUpVector(-toCenter,true);
            // }

        }
    }
    private void OnTriggerStay(Collider collision)
    {
        if(collision.GetComponent<Rigidbody>()){
            Rigidbody objRig = collision.GetComponent<Rigidbody>();
            objRig.useGravity=false;
            Vector3 toCenter = (this.transform.position-collision.transform.position).normalized;
            objRig.AddForce(toCenter*gravityPower,ForceMode.Acceleration);

            PlayerControl pctrl;
            if(collision.TryGetComponent<PlayerControl>(out pctrl)){
                Vector2Int StayCnt;
                if(!playerStayCnt.TryGetValue(pctrl,out StayCnt)){
                    StayCnt=Vector2Int.zero;
                }
                StayCnt.x++;
                playerStayCnt[pctrl]=StayCnt;
                
                pctrl.radiusUpVector=radius;
                pctrl.setUpVector(-toCenter);
				// this.transform.position-=playerVectorUp*(rig.velocity.magnitude*Mathf.Sin(changeAngle)*0.05f);
            }
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if(collision.GetComponent<Rigidbody>()){
            Rigidbody objRig = collision.GetComponent<Rigidbody>();
            objRig.useGravity=true;

            // PlayerControl pctrl;
            // if(collision.TryGetComponent<PlayerControl>(out pctrl)){
            //     pctrl.setUpVector(defaultUp,true);
            // }
        }
    }
    IEnumerator AfterFixedUpdateCoroutine(){
		yield return new WaitForFixedUpdate();
		AfterFixedUpdate();
	}
	void AfterFixedUpdate(){
        List<PlayerControl> players = new List<PlayerControl>(playerStayCnt.Keys);
        foreach (PlayerControl key in players){
            Vector2Int StayCnt = playerStayCnt[key];
            if(StayCnt.x!=StayCnt.y){
                if(StayCnt.x==0){ // トリガーから出たとき
                    key.setUpVector(defaultUp,true);
                }else if(StayCnt.y==0){ // トリガーに入ったとき
                    Vector3 toCenter = (this.transform.position-key.transform.position).normalized;
                    key.setUpVector(-toCenter,true);
                }
            }

            StayCnt.y=StayCnt.x;
            playerStayCnt[key]=StayCnt;
        }
	}
}
