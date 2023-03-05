using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetRenderTex : MonoBehaviour
{
	const string mainTex = "_MainTex"; 
	public enum SourceType
	{
		Camera,
		RenderTexture,
	}
	public enum TargetType
	{
		MeshRenderer,
		RawImage,
	}
	public bool enableTexName=false,enableUpdateTiles=false;
	public string textureName;
	public TargetType targetType;
	public MeshRenderer mesh;
	public RawImage image;
	public SourceType sourceType;
	public Camera cam;
	public RenderTexture renderTex;
	Material material;
	public Vector2 tiling = Vector2.one;
	public Vector2 offset = Vector2.zero;
	Vector2 befTiling,befOffset;
	// Start is called before the first frame update
	void Start()
	{
		switch (sourceType){
			case SourceType.Camera:

				renderTex=cam.targetTexture;

				break;
			case SourceType.RenderTexture:
				break;
			default:
				break;
		}
		switch (targetType){
			case TargetType.MeshRenderer:
				if(mesh==null){
					material = GetComponent<MeshRenderer>().material;
				}else{
					material=mesh.material;
				}
				material = new Material(material);
				GetComponent<MeshRenderer>().material=material;
				break;
			case TargetType.RawImage:
				if(image==null){
					material = GetComponent<RawImage>().material;
				}else{
					material = image.material;
				}
				material = new Material(material);
				GetComponent<RawImage>().material=material;
				break;
			default:
				break;
		}
		

		
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
