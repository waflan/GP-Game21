using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBlocks3D : MonoBehaviour
{
    public BrockBreakerMng mng;
    bool isBlockSet=false;
    public GameObject blockPrefub;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isBlockSet&&mng.blockCnt>0){
            setBlock();
            isBlockSet=true;
        }
    }
    void setBlock(){
        Vector3 pos = this.transform.position;
        Quaternion rot = this.transform.rotation;
        Vector3 scale=this.transform.localScale;
        this.transform.localScale=Vector3.one;
        this.transform.rotation=Quaternion.identity;
        foreach (Transform blockT in mng.blocks)
        {
            GameObject block = Instantiate(blockPrefub,blockT.localPosition+pos,Quaternion.identity);
            block.transform.localScale=blockT.localScale;
            block.transform.SetParent(transform);
            blockT.GetComponent<BB_Block>().Block3d=block.transform;
        }
        this.transform.rotation=rot;
        this.transform.localScale=scale;
    }
}
