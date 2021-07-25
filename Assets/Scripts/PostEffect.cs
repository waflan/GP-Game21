using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffect : MonoBehaviour
{
    public Shader shader;
    public Color outLineCol=new Color(0,0,0);

    Material material;
    Camera cam;
    void Start(){
        cam=GetComponent<Camera>();
        cam.depthTextureMode|=DepthTextureMode.DepthNormals;
        material=new Material(shader);
        material.SetColor("_OutLineColor",outLineCol);
    }

	void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit (src, dest, material);
        material.SetColor("_OutLineColor",outLineCol);

	}
}
