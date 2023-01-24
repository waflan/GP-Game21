using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidEffectSound : MonoBehaviour
{    
    const float impulseThreshold=6;
    Rigidbody rig;
    AudioSource audio;
    bool playFlag=false;
    void Start(){
        audio = GetComponent<AudioSource>();
        rig = GetComponent<Rigidbody>();
    }
    void Update(){
        if(playFlag){
            playFlag=false;
            audio.pitch = Random.Range(0.9f,1.1f);
            audio.Play();
        }
    }

    void OnCollisionEnter(Collision collision){
        if(!playFlag){
            if(collision.impulse.magnitude>impulseThreshold){
                playFlag=true;
            }
            // Debug.Log(rig.velocity.magnitude+"&"+collision.impulse.magnitude);
        }
        
    }
}
