using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRain : MonoBehaviour
{
    public Vector3 p1,p2;
    public float cloneCoolTime =0.3f;
    public float objectDeathTime =5;
    public GameObject prefubObject;
    float time=0;    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time+=Time.deltaTime;
        if(time>cloneCoolTime){
            time-=cloneCoolTime;
            GameObject obj = Instantiate(prefubObject,randomVector3(p1,p2),Quaternion.identity);
            Destroy(obj,objectDeathTime);
        }
    }

    Vector3 randomVector3(Vector3 point1,Vector3 point2){
        return new Vector3(Random.Range(point1.x,point2.x),Random.Range(point1.y,point2.y),Random.Range(point1.z,point2.z));
    }
}
