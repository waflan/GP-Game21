using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRenderTex : MonoBehaviour
{
    public Camera cam;
    public Material material;
    public RenderTexture renderTex;
    // Start is called before the first frame update
    void Start()
    {   
        cam.gameObject.SetActive(false);
        cam.gameObject.SetActive(true);
        material=GetComponent<MeshRenderer>().material;
        material=new Material(material);
        material.SetTexture("_MainTex",cam.targetTexture);

        GetComponent<MeshRenderer>().material=material;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
