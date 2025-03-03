using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilingMove : MonoBehaviour
{
    public Material material;
    public string targetTexture="_MainTex";
    public Vector2 offsetSpeed=new Vector2(0,1);
    public bool makeInstance=false;
    
    // Start is called before the first frame update
    void Start()
    {
        
        if(material==null){
            if(makeInstance){
                material=GetComponent<MeshRenderer>().material;
            }else{
                material=GetComponent<MeshRenderer>().sharedMaterial;
            }
        }else{
            if(makeInstance){
                material=new Material(material);
            }
            GetComponent<MeshRenderer>().material=material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = material.GetTextureOffset(targetTexture);
        offset += offsetSpeed*Time.deltaTime;
        offset.x = Mathf.Repeat(offset.x,1);
        offset.y = Mathf.Repeat(offset.y,1);
        material.SetTextureOffset(targetTexture,offset);
    }
}
