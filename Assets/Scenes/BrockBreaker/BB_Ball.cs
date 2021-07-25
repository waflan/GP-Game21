using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BB_Ball : MonoBehaviour
{
    public BrockBreakerMng mng;
    Rigidbody2D rig;

    public AudioSource missAudio;
    public AudioSource hitAudio;
    Vector3 rigVel;
    bool isPlay;
    
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        this.tag="Ball";
        rig=GetComponent<Rigidbody2D>();
        rigVel=rig.velocity;
        isPlay=mng.isPlay;
        // Debug.Log("hoge");
    }

    // Update is called once per frame
    void Update()
    {
        if(mng.isPlay){
            if(!isPlay){
                rig.velocity=rigVel;
            }
            if(rig.velocity.sqrMagnitude<speed*speed){
                rig.velocity=rig.velocity.normalized*speed;
            }
        }else {
            if(isPlay){
                rigVel=rig.velocity;
            }
            rig.velocity=Vector2.zero;
        }
        isPlay=mng.isPlay;
    }

    void OnCollisionEnter2D(Collision2D collision){
        
        if(collision.transform.tag=="Killer"){
            if(missAudio!=null){
                Destroy(this.gameObject,missAudio.clip.length);
                this.GetComponent<Collider2D>().enabled=false;
                this.GetComponent<SpriteRenderer>().enabled=false;
                missAudio.Play();
            }else{
                Destroy(this.gameObject);
            }
            mng.ballCnt--;
        }else if(collision.transform.tag=="GimmPlayer"){
            if(rig==null){
                rig=GetComponent<Rigidbody2D>();
            }
            float spd = rig.velocity.magnitude;
            rig.velocity=(this.transform.position-collision.gameObject.transform.position).normalized*spd;
        }else if(collision.transform.tag=="Block"){
            collision.gameObject.SetActive(false);
            if(hitAudio!=null){
                hitAudio.Play();
            }
        }
    }
}
