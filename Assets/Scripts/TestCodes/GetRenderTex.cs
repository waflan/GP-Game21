using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRenderTex : MonoBehaviour
{
    const string mainTex = "_MainTex"; 
    public enum TargetType
    {
        Camera,
        RenderTexture,
    }
    public bool enableTexName=false,enableUpdateTiles=false;
    public string textureName;
    public TargetType targetType;
    public Camera cam;
    public RenderTexture renderTex;
    Material material;
    public Vector2 tiling = Vector2.one;
    public Vector2 offset = Vector2.zero;
    Vector2 befTiling,befOffset;
    // Start is called before the first frame update
    void Start()
    {
        switch (targetType){
            case TargetType.Camera:

                renderTex=cam.targetTexture;

                break;
            case TargetType.RenderTexture:



                break;
            default:
                break;
        }
        material = GetComponent<MeshRenderer>().material;
        material = new Material(material);
        if(enableTexName){
            material.SetTexture(textureName, renderTex);
            if(enableUpdateTiles){
                material.SetTextureScale(textureName,tiling);
                material.SetTextureOffset(textureName,offset);
            }
        }else{
            material.SetTexture(mainTex, renderTex);
            if(enableUpdateTiles){
                material.SetTextureScale(mainTex,tiling);
                material.SetTextureOffset(mainTex,offset);
            }
        }

        befTiling=tiling;
        befOffset=offset;
        GetComponent<MeshRenderer>().material = material;

    }

    // Update is called once per frame
    void Update()
    {
        if(enableUpdateTiles){
            UpdateTilings();
        }
    }
    void UpdateTilings(){
        if(befTiling!=tiling){
                material.SetTextureScale((enableTexName?textureName:mainTex),tiling);
                befTiling=tiling;
            }
            if(befOffset!=offset){
                material.SetTextureOffset((enableTexName?textureName:mainTex),offset);
                befOffset=offset;
            }
    }
}
