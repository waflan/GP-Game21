using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimeEventController : MonoBehaviour
{
    public Vector3 offset=new Vector3();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition=offset;
        transform.localRotation=Quaternion.identity;
    }
    public void OnCallChangeFace(){

    }
}
