using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BB_Block : MonoBehaviour
{
    public Transform Block3d;
    public BrockBreakerMng mng;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnEnable(){
        if(Block3d!=null){
            Block3d.gameObject.SetActive(true);
        }
        if(mng!=null){
            mng.blockCnt++;
            
        }
        
    }
    void OnDisable(){
        if(Block3d!=null){
            Block3d.gameObject.SetActive(false);
        }
        if(mng!=null){
            mng.blockCnt--;
        }
    }    
}
