using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyingMaterial : MonoBehaviour
{
    public Material material;
    public bool makeInstance=false;
    public TableTexture textures = new TableTexture();
    public TableColor colors = new TableColor();
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
        
        
        foreach (TableTexturePair pair in textures.GetList())
        {
            material.SetTexture(pair.Key,pair.Value);
        }
        foreach (TableColorPair pair in colors.GetList())
        {
            material.SetColor(pair.Key,pair.Value);
        }
    }

}
