using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BB_Ball : MonoBehaviour
{
    public BrockBreakerMng mng;
    // Start is called before the first frame update
    void Start()
    {
        this.tag="Ball";
        // Debug.Log("hoge");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision){
        
        if(collision.transform.tag=="Killer"){
            Destroy(this.gameObject);
            mng.ballCnt--;
        }else if(collision.transform.tag=="GimmPlayer"){

        }
    }
}
