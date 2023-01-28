using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour{
	PlayerControl player;
	Transform playerAvatarParent;
	public List<GameObject> playerAvatarPrefabs;
	public bool viewEnabled = true;
	public Transform previewParent;
	Transform preview;
	public float rotateSpeed=30;
	public int nowNumber=0;

	void Start(){
		GameObject playerObj;
		if ((playerObj = GameObject.FindGameObjectWithTag("Player")) != null){
			player = playerObj.GetComponent<PlayerControl>();
			if ((playerAvatarParent = playerObj.transform.Find("AvatarParent")) == null){
				playerAvatarParent = new GameObject("AvatarParent").transform;
				playerAvatarParent.SetParent(playerObj.transform);
				playerAvatarParent.localPosition = Vector3.zero;
				playerAvatarParent.localRotation = Quaternion.identity;
			}
		}
		
		previewParent = new GameObject("PreviewParent").transform;
		previewParent.SetParent(transform);
		previewParent.localPosition = Vector3.zero;
		previewParent.localRotation = Quaternion.identity;
		previewParent.gameObject.layer=gameObject.layer;
	}

	void Update(){
		if (!enabled){
			return;
		}
		if (viewEnabled){
			if (preview == null){
				setPreview(0);
			}
			previewParent.rotation *= Quaternion.AngleAxis(rotateSpeed * Time.deltaTime, preview.up);
		}
	}
	public void NumberNext(){
		nowNumber++;
		nowNumber%=playerAvatarPrefabs.Count;
	}
	public void NumberPrev(){
		nowNumber--;
		if(nowNumber<0)nowNumber+=playerAvatarPrefabs.Count;
	}

	public void setPreview(){
		setPreview(nowNumber);
	}
	public void setPreview(int index){
		if (index < 0 || index >= playerAvatarPrefabs.Count){
			return;
		}
		if(preview!=null){
			GameObject.Destroy(preview.gameObject);
		}
		preview = GameObject.Instantiate(playerAvatarPrefabs[index], transform.position, transform.rotation).transform;
		preview.SetParent(previewParent);
		MyFoundation.setLayerAll(preview,gameObject.layer);
	}

	public void setAvatar(){
		setAvatar(nowNumber);
	}
	public void setAvatar(int index)
	{
		if (index < 0 || index >= playerAvatarPrefabs.Count){
			return;
		}
		foreach (Transform child in playerAvatarParent){
			GameObject.Destroy(child.gameObject);
		}
		Transform avaterNew = GameObject.Instantiate(playerAvatarPrefabs[index], playerAvatarParent.position, playerAvatarParent.rotation).transform;
		avaterNew.SetParent(playerAvatarParent);
		// avaterNew.gameObject.AddComponent<AnimeEventController>().offset = new Vector3(0,-0.5f,0);

		player.animator = avaterNew.GetComponent<Animator>();
		player.reloadSkins();
		// player.animator
	}
	public void hidePlayerMesh(){
		player.hideShowMesh(false);
	}
}
