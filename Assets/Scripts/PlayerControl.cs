using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
	public KeyConfig keyConfig;
	public int controlMode=-1;
	int befCtrlMode;
	public int playingMode;
	public bool actionable=true;
	public Transform CamTransformParent;
	public Transform CamTransform;

	Rigidbody rig;

	public Animator animator = new Animator();
	public CapsuleCollider characterCollider = new CapsuleCollider();
	public PlayerSquatTrigger SquatTrigger;
	public Canvas Menu = new Canvas();

	// デバッグ用変数



	// ここからアクション用変数

		// ローカル変数
	float rx=0,ry=0; // カメラ方向(横,縦)
	Vector2 rotOffset=new Vector2();
	bool cursorLock=true; // 
	float camDist=1;
	bool doStop1fCam=false;
	Vector2 move =new Vector3();
	float moveDirection,befMoveDirection=0;
	Vector3 movingVelocity=new Vector3();
	
	// [System.NonSerialized]
	public bool onGround,jump,isRoll,befRoll,isSquat,isSquatRoll,isDive,isMove,befGround,isMenu,isFocus,isCheck;
	[System.NonSerialized]
	public float radiusUpVector;
	float tensorUp;
	float jumpTime,rollTime;
	Vector3 GroundVelocity=new Vector3(),GroundAngVelocity=new Vector3();
	public Vector3 fieldAngVelocity=new Vector3();
	Vector3 beforeUpVector=new Vector3();
	List<SkinnedMeshRenderer> skins=new List<SkinnedMeshRenderer>();
	List<Transform> focusTargets=new List<Transform>();
	public List<Transform> checkTargets=new List<Transform>();
	Transform focusTarget,checkTarget,checkCamPos;
	float ccoliderHeight;

		// パブリック変数
	public float moveSpeed=7; // 移動速度
	public float squatMoveSpeed=2; // しゃがみ移動速度
	[Range (0,1)]
	public float footGripTime=.1f; // 地上での移動速度更新にかかる時間＆慣性速度の抵抗
		[Range (0,3)]
	public float airGripTime=.5f; // 空中での移動速度更新にかかる時間
	[Range (0,1)]
	public float rotateTime=.2f; // プレイヤーの操作による回転の時間
	public Vector2 rotateSpeed=new Vector2(270,180); // カメラ回転速度
	public Vector2 mouseRotateSpeed=new Vector2(5,5); // マウスでのカメラ回転速度
	public Vector2 camDistanceRange=new Vector2(1,10); // カメラの距離範囲
	[Range(-2,2)]
	public float camUpOffset = 0.5f; //カメラの中心を上にずらす量を指定
	public Vector3 playerUpVector=Vector3.up; // プレイヤーの上方向(デフォルト：y軸方向)
	[Range (0.1f,5)]
	public float jumpMaxTime=1; // ジャンプの加速時間
	public float firstJumpForce=300; // ジャンプ初動の力
	public float secondJumpForce=200; // ジャンプ継続時の加速度
	public float rollCoolTime=.8f; // 前転のクールタイム
	public float squatRollCoolTime=.4f; // 前転のクールタイム
	public float rollForce=300; // 前転の力
	public float diveForce=300; // 飛び込みの力
	public bool able2Check=false; // 調べる機能のフラグ(外部から切り替え)

	public bool stopOnStart=false;

	void Start(){
		if(stopOnStart){
			Time.timeScale=0;
		}
		if(keyConfig==null){
			if(!TryGetComponent<KeyConfig>(out keyConfig)){
				keyConfig = gameObject.AddComponent<KeyConfig>();
			}
		}
		rig=transform.GetComponent<Rigidbody>();
		beforeUpVector=playerUpVector;
		skins.AddRange(animator.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>());
		ccoliderHeight=characterCollider.height;
		befCtrlMode=controlMode;
		tensorUp=rig.inertiaTensor.y;
	}
	void FixedUpdate(){
		rig.inertiaTensor=new Vector3(1e24f,tensorUp,1e24f);
		// Debug.Log(fieldAngVelocity);
		StartCoroutine(AfterFixedUpdateCoroutine());
	}
	IEnumerator AfterFixedUpdateCoroutine(){
		yield return new WaitForFixedUpdate();
		AfterFixedUpdate();
	}
	void AfterFixedUpdate(){
		// fieldAngVelocity=Vector3.zero;
	}
	void Update(){

		// 上方ベクトルへの回転
		Quaternion UpVectorRotate = RotateFromUpVector(playerUpVector);
		// Quaternion UpVectorRotate = Quaternion.FromToRotation(Vector3.up,playerUpVector);

		// メニュー表示
		if(Input.GetKeyDown(keyConfig.menu)){
			setMenu(isMenu^true);
			
		}
		
		// 操作モード切り替え
		if(Input.GetKeyDown(keyConfig.modeChange)&&isMenu&&isCheck){
			if(playingMode==0){
				playingMode=1;
			}else if(playingMode==1){
				playingMode=0;
			}
		}

		// フォーカス対象のリスト
		focusTargets.RemoveAll(t=>t==null);
		foreach (GameObject target in GameObject.FindGameObjectsWithTag("FocusTarget"))
		{
			if(!focusTargets.Contains(target.transform)){
				focusTargets.Add(target.transform);
			}
		}

		// 調べる対象のリスト
		checkTargets.RemoveAll(t=>t==null);
		foreach (GameObject target in GameObject.FindGameObjectsWithTag("CheckTarget"))
		{
			if(!checkTargets.Contains(target.transform)){
				checkTargets.Add(target.transform);
			}
		}

		// チェック切り替え
		if(Input.GetKeyDown(keyConfig.check)){
			// Debug.Log(isCheck);
			if(isCheck){
				isCheck=false;
				controlMode=befCtrlMode;
				hideShowMesh(true);
			}else if(checkTargets.Count>0&&able2Check){
				isCheck=true;
				hideShowMesh(false);
				befCtrlMode=controlMode;
				controlMode=2;
				Vector3 pos=this.transform.position;
				checkTargets.Sort((t1,t2) => (int)((t1.position-pos).sqrMagnitude-(t2.position-pos).sqrMagnitude));
				checkTarget=checkTargets[0];
				if(checkTarget.GetComponent<CheckObj>()){
					checkCamPos=checkTarget.GetComponent<CheckObj>().child;
				}else if(checkTarget.childCount==0){
					checkCamPos=checkTarget;
				}else{
					checkCamPos=checkTarget.GetChild(0);
				}
			}
		}

		// カーソル状態の変更
		if(isCheck||isMenu){
			Cursor.lockState=CursorLockMode.None;
		}else if(actionable){
			Cursor.lockState=CursorLockMode.Locked;
		}

		if(actionable&&!isMenu){
			// 移動量取得
				// wsadキー移動
			move=Vector2.zero;
			if(Input.GetKey(keyConfig.wsad[3])){
				move.x+=1;
			}
			if(Input.GetKey(keyConfig.wsad[2])){
				move.x-=1;
			}
			if(Input.GetKey(keyConfig.wsad[0])){
				move.y+=1;
			}
			if(Input.GetKey(keyConfig.wsad[1])){
				move.y-=1;
			}
			isMove=(move!=Vector2.zero);
			if(isMove){
				befMoveDirection=moveDirection;
			}
			moveDirection=Mathf.Rad2Deg*Mathf.Atan2(move.x,move.y);

			// 回転量取得
				// 矢印キー回転
			Vector2 rMove=Vector2.zero;
			if(Input.GetKey(KeyCode.RightArrow)){
				rMove.x+=rotateSpeed.x*Time.deltaTime;
			}
			if(Input.GetKey(KeyCode.LeftArrow)){
				rMove.x-=rotateSpeed.x*Time.deltaTime;
			}
			if(Input.GetKey(KeyCode.UpArrow)){
				rMove.y-=rotateSpeed.y*Time.deltaTime;
			}
			if(Input.GetKey(KeyCode.DownArrow)){
				rMove.y+=rotateSpeed.y*Time.deltaTime;
			}
				// マウス回転
			if(cursorLock){
				rMove.x+=Input.GetAxis("Mouse X")*mouseRotateSpeed.x*Mathf.Pow(2,keyConfig.cursorSpeed);
				rMove.y-=Input.GetAxis("Mouse Y")*mouseRotateSpeed.y*Mathf.Pow(2,keyConfig.cursorSpeed);
			}

			// フォーカスの切り替え
			if(Input.GetKeyDown(keyConfig.focus)){
				if(isFocus){
					isFocus=false;
					rotOffset=Vector2.zero;
				}else if(focusTargets.Count>0){
					isFocus=true;
					Vector3 pos=this.transform.position;
					focusTargets.Sort((t1,t2)=> (int)((t1.position-pos).sqrMagnitude-(t2.position-pos).sqrMagnitude));
					focusTarget=focusTargets[0];
				}
			}
			
			// 回転情報の更新
			if(isFocus){
				if(focusTarget==null){
					isFocus=false;
					rotOffset=Vector2.zero;
				}else{
					Vector3 focusLocalVec = Quaternion.Inverse(UpVectorRotate)*(focusTarget.position-this.transform.position).normalized;
					rx=Mathf.Rad2Deg*Mathf.Atan2(focusLocalVec.x,focusLocalVec.z);
					ry=Mathf.Asin(focusLocalVec.y);
					rotOffset+=rMove;
					rotOffset.y=Mathf.Clamp(rotOffset.y,-30,30);
					rotOffset.x=Mathf.Clamp(rotOffset.x,-30,30);
					rx+=rotOffset.x;
					ry+=rotOffset.y;
				}
			}else{
				rx+=rMove.x*(keyConfig.rotateInverseX?-1:1);
				ry+=rMove.y*(keyConfig.rotateInverseY?-1:1);
			}
			ry=Mathf.Clamp(ry,-90,90);
			rx=Mathf.Repeat(rx+180,360)-180;

			// 前転＆しゃがみ
			if(Input.GetKey(keyConfig.roll)){
				// Vector3 horizontalVel=rig.velocity-(playerUpVector.normalized*Vector3.Dot(playerUpVector.normalized,rig.velocity));
				if(!isRoll){
					if(isMove){
						if(Input.GetKeyDown(keyConfig.roll)&&!isSquat){
							isRoll=true;
							rollTime=rollCoolTime;
						}else if(isSquat&&Input.GetKeyDown(keyConfig.jump)){
							isRoll=isSquatRoll=true;
							rollTime=squatRollCoolTime;
						}
					}else{
						if(!onGround&&Input.GetKeyDown(keyConfig.roll)){
							isRoll=true;
							rollTime=rollCoolTime;
						}else if(SquatTrigger.isOn&&Input.GetKeyDown(keyConfig.jump)){
							isRoll=isSquatRoll=true;
							rollTime=squatRollCoolTime;
						}
					}
				}
				isSquat=onGround;
			}else{
				if(!isRoll&&isSquat&&Input.GetKeyDown(keyConfig.jump)){
					// Debug.Log("hoge");
					isRoll=isSquatRoll=true;
					rollTime=squatRollCoolTime;
				}
				isSquat=onGround&&SquatTrigger.isOn;
			}
			
		}

		if(controlMode==-1){ // デバッグ用テスト
			
			// 指定ベクトル基準で回転
			Quaternion CamRotate = Quaternion.FromToRotation(Vector3.up,playerUpVector)* Quaternion.AngleAxis(rx,Vector3.up)*Quaternion.AngleAxis(ry,Vector3.right);
			Quaternion toRotate = Quaternion.FromToRotation(Vector3.up,playerUpVector)*Quaternion.AngleAxis(rx+Mathf.Rad2Deg*Mathf.Atan2(move.x,move.y),Vector3.up);
			CamTransformParent.rotation = CamRotate;
			CamTransformParent.position = this.transform.position+CamTransformParent.rotation*(Vector3.back*5);
			this.transform.rotation =toRotate;
			if(move!=Vector2.zero){
				rig.velocity=toRotate*Vector3.forward*moveSpeed;
			}else{
				rig.velocity=Vector3.zero;
			}
			

		}
		else if(controlMode==0||controlMode==1){ // 一人称＆三人称視点操作

			// 1f子カメラを止める(座標を一次保存＆最後戻す)
			Vector3 CamPosOnStart= CamTransform.position;
			// ずれた子カメラの座標＆回転を視点へ遷移させる。
			if(CamTransform.localPosition!=Vector3.zero){
				CamTransform.localPosition=Vector3.Lerp(CamTransform.localPosition,Vector3.zero,10*Time.deltaTime);
			}
			if(CamTransform.localRotation!=Quaternion.identity){
				CamTransform.localRotation=Quaternion.Lerp(CamTransform.localRotation,Quaternion.identity,5*Time.deltaTime);
			}

			// 軸が更新されたときにカメラ回転の値を更新
			if(beforeUpVector!=playerUpVector){
				// if(playerUpVector.y>0){
				// 	rx-=Mathf.Rad2Deg*Mathf.Atan2(playerUpVector.x,playerUpVector.z)-Mathf.Rad2Deg*Mathf.Atan2(beforeUpVector.x,beforeUpVector.z);
				// }else{
				// 	rx+=Mathf.Rad2Deg*Mathf.Atan2(playerUpVector.x,playerUpVector.z)-Mathf.Rad2Deg*Mathf.Atan2(beforeUpVector.x,beforeUpVector.z);
				// }
				
				rx=nextLocalRot(rx,befMoveDirection,playerUpVector,beforeUpVector);
				transform.rotation*=Quaternion.FromToRotation(beforeUpVector,playerUpVector);
				
				beforeUpVector=playerUpVector;
			}

			// 指定ベクトル基準で回転
			Quaternion CamRotate = UpVectorRotate* Quaternion.AngleAxis(rx,Vector3.up)*Quaternion.AngleAxis(ry,Vector3.right);
			Quaternion toRotate = UpVectorRotate*Quaternion.AngleAxis(rx+moveDirection,Vector3.up);
			CamTransformParent.rotation = CamRotate;
			
			if(controlMode==0){ // 三人称視点の場合
				hideShowMesh(true);
				// カメラをプレイヤー後方に移動させる
				var ray= new Ray(this.transform.position,CamRotate*Vector3.back);
				var hit= new RaycastHit();
				float camToDist;
				if(Physics.Raycast(ray,out hit,camDistanceRange.y,(1))){
					camToDist=Mathf.Max(camDistanceRange.x,hit.distance);
				}else{
					camToDist=camDistanceRange.y;
				}
				if(camDist>camToDist){
					camDist=Mathf.Lerp(camDist,camToDist,10f*Time.deltaTime);
				}else{
					camDist=Mathf.Lerp(camDist,camToDist,4f*Time.deltaTime);
				}
				CamTransformParent.position=this.transform.position+playerUpVector*((90-Mathf.Abs(ry))/90)*camUpOffset+(CamRotate*Vector3.back)*camDist;
			}else{ // 一人称視点の場合
				CamTransformParent.position=this.transform.position;
				hideShowMesh(false);
			}
			// カメラを1f止める場合子カメラを前フレームに戻す
			if(doStop1fCam){
				CamTransform.position=CamPosOnStart;
				doStop1fCam=false;
			}
			
			// Debug.Log(positionToCamRotate(CamTransFormParent.position,transform.position,playerUpVector)+$":{rx},{ry}");

			// 移動方向に合わせてプレイヤーを回転する
			if(isMove){
				transform.rotation=Quaternion.Lerp(transform.rotation,toRotate,Time.deltaTime/rotateTime);
				// transform.rotation=toRotate;
			}else{
				Quaternion fixRotate = Quaternion.FromToRotation(transform.rotation*Vector3.up,playerUpVector);
				transform.rotation=fixRotate*transform.rotation;
			}
			if(onGround){
				rig.angularVelocity=Vector3.Lerp(rig.angularVelocity,GroundAngVelocity,Time.deltaTime/footGripTime);
			}else if(isMove){
				rig.angularVelocity=Vector3.zero;
			}

			// 移動方向に合わせプレイヤーの速度を更新
				// 操作による速度を除く
			Vector3 velocity = rig.velocity-movingVelocity;
				// 操作による速度を更新
			if((isMove&&!(isSquat&&!isRoll))||(SquatTrigger.isOn&&isRoll)){
				// movingVelocity=toRotate*Vector3.forward*speed;
				movingVelocity=Vector3.Lerp(movingVelocity,toRotate*Vector3.forward*moveSpeed,Time.deltaTime/(onGround?footGripTime:airGripTime));
			}else if(isMove){
				movingVelocity=Vector3.Lerp(movingVelocity,toRotate*Vector3.forward*squatMoveSpeed,Time.deltaTime/(onGround?footGripTime:airGripTime));
			}else{
				// movingVelocity=Vector3.zero;
				movingVelocity=Vector3.Lerp(movingVelocity,Vector3.zero,Time.deltaTime/(onGround?footGripTime:airGripTime));
			}
				// 地上にいる間は元の速度を相殺する
			if(onGround){
				velocity=Vector3.Lerp(velocity,GroundVelocity,Time.deltaTime/footGripTime);
			}
				// 操作による速度を適用
			rig.velocity = velocity + movingVelocity;

			// 地面で動いていない際に摩擦係数をつける
			characterCollider.material.dynamicFriction=characterCollider.material.staticFriction=(onGround&&!isMove)?1:0;

			// ジャンプの処理
			if(onGround&&((!isSquat||!isMove)&&!SquatTrigger.isOn)&&Input.GetKeyDown(keyConfig.jump)){
				jump=true;
				jumpTime=jumpMaxTime;
				onGround=false;
				rig.AddForce(playerUpVector*firstJumpForce,ForceMode.Acceleration);
				// animator.SetBool("Jump",true);
			}
			if(jump){
				if(Input.GetKey(keyConfig.jump)){
					if(jumpTime>=0){
						rig.AddForce(playerUpVector* secondJumpForce*Time.deltaTime,ForceMode.Acceleration);
					}else{
						jump=false;
					}
				}else{
					jump=false;
					jumpTime=0;
				}
			}
			jumpTime-=Time.deltaTime;

			// 前転の処理
			if(isRoll){
				minimizeCollider(true);
				if(!befRoll){
					animator.SetTrigger("Roll");
					if(!onGround){
						isDive=true;
						if(isMove){
							rig.AddForce(toRotate*Vector3.forward*diveForce,ForceMode.Acceleration);
						}else{
							rig.AddForce(transform.rotation*Vector3.forward*diveForce,ForceMode.Acceleration);
						}
					}else{
						if(isMove){
							rig.AddForce(toRotate*Vector3.forward*rollForce,ForceMode.Acceleration);
						}else{
							rig.AddForce(transform.rotation*Vector3.forward*rollForce,ForceMode.Acceleration);
						}
					}
				}
				if(animator.GetCurrentAnimatorStateInfo(0).IsName("SquatRoll")){
					isSquatRoll=false;
				}
				if(!isSquatRoll&&animator.GetCurrentAnimatorStateInfo(0).IsName("Squat")){
					rollTime=0;
				}
				if(rollTime<=0){
						isRoll=false;
						minimizeCollider(isSquat);
					}
				if(onGround){
					if(isDive){
						isDive=false;
						rollTime=.4f;
					}
					rollTime-=Time.deltaTime;
					
				}else{
					if(!isDive){
						rollTime=0;
					}
				}
			}else{
				if(!befRoll){
					isSquatRoll=false;
					minimizeCollider(isSquat);
				}
			}
			befRoll=isRoll;
			// Debug.Log(Input.GetKey(keyConfig.jump)+" "+jump+" "+ jumpTime+" "+Vector3.Dot(playerVectorUp.normalized,rig.velocity));

		}
		else if(controlMode==2){
			CamTransform.position=Vector3.Lerp(CamTransform.position,checkCamPos.position,10*Time.deltaTime);
			CamTransform.rotation=Quaternion.Lerp(CamTransform.rotation,checkCamPos.rotation,10*Time.deltaTime);
			rig.velocity=Vector3.zero;
		}

		// アニメーションの処理
		if(onGround){
			if(jump&&!animator.GetBool("Jump")){
				animator.SetBool("Jump",true);
			}
			if(!jump){
				animator.SetBool("Jump",false);
			}
		}else{
			if(jump){
				animator.SetBool("Jump",true);
			}else{
				if(befGround){
					animator.SetTrigger("Fall");
				}
			}
		}
		animator.SetBool("Running",(move!=Vector2.zero));
		animator.SetBool("OnGround",onGround);
		animator.SetBool("Squat",isSquat);
		befGround=onGround;
	}
	private void OnTriggerStay(Collider collision)
	{
		// Debug.Log(collision.transform.name);
		if(collision.isTrigger){

		}else{
			onGround=true;
			if(Vector3.Dot(playerUpVector.normalized,rig.velocity)<=0.1){
				jump=false;
			}
			Rigidbody colRig;
			if(collision.gameObject.TryGetComponent<Rigidbody>(out colRig)){
				GroundVelocity = colRig.velocity+Vector3.Cross(colRig.angularVelocity,(transform.position-colRig.transform.position));
				
				GroundAngVelocity = colRig.angularVelocity;

			}else{
				GroundVelocity=GroundAngVelocity=Vector3.zero;
			}
		}
		
	}
	public void reloadSkins(){
		skins.Clear();
		skins.AddRange(animator.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>());
	}
	private void OnTriggerExit(Collider collision)
	{
		if(collision.isTrigger){

		}else{
			onGround=false;
		}
	}
	public void hideShowMesh(bool value){
		foreach (SkinnedMeshRenderer mesh in skins)
		{
			if(value){
				mesh.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.On;
			}else{
				mesh.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
			}
			mesh.enabled=value;
		}
	}

	public void setMenu(bool b){
		isMenu=b;
		Menu.gameObject.SetActive(isMenu);
		
		if(isMenu){
			Time.timeScale=0;
		}else{
			Time.timeScale=1;
		}
	}
	public void warpTo(Vector3 pos){
		this.transform.position=pos;
	}

	public void ExitGame(){
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#elif UNITY_STANDALONE
		UnityEngine.Application.Quit();
		#elif UNITY_WEBGL
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		#endif
	}

	Quaternion RotateFromUpVector(Vector3 up){
        return Quaternion.AngleAxis(Mathf.Rad2Deg*Mathf.Atan2(up.x,up.z),Vector3.up)*Quaternion.AngleAxis(Vector3.Angle(Vector3.up,up),Vector3.right);
    }

	float nextLocalRot(float localRot,float moveDir,Vector3 nowUp,Vector3 befUp){
        float result=Mathf.Repeat(localRot+moveDir+180,360)-180;

        float lati1,lati2,between;
        lati1 = (90-Vector3.Angle(Vector3.down,befUp))*Mathf.Deg2Rad;
        lati2 = (90-Vector3.Angle(Vector3.down,nowUp))*Mathf.Deg2Rad;
        between = Vector3.Angle(befUp,nowUp)*Mathf.Deg2Rad;

        if(lati1!=lati2){
            float dir1,dir2;
            dir1 = Mathf.Acos((Mathf.Sin(lati2)-Mathf.Sin(lati1)*Mathf.Cos(between))/(Mathf.Cos(lati1)*Mathf.Sin(between)));
            dir2 = Mathf.Acos((Mathf.Sin(lati2)*Mathf.Cos(between)-Mathf.Sin(lati1))/(Mathf.Cos(lati2)*Mathf.Sin(between)));
            if(result>0){
                result+=(dir2-dir1)*Mathf.Rad2Deg;
            }else{
                result-=(dir2-dir1)*Mathf.Rad2Deg;
            }
            if(float.IsNaN(dir1)||float.IsNaN(dir2)){
                float deltaHolAngle = (Mathf.Rad2Deg*Mathf.Atan2(nowUp.x,nowUp.z)-Mathf.Rad2Deg*Mathf.Atan2(befUp.x,befUp.z));
                deltaHolAngle = Mathf.Repeat(deltaHolAngle+180,360)-180;
                if(nowUp.y>0){
                    result=localRot+moveDir-deltaHolAngle;
                }else{
                    result=localRot+moveDir+deltaHolAngle;
                }
            }
        }
        return Mathf.Repeat(result-moveDir+180,360)-180;
    }

	Vector2 positionToCamRotate(Vector3 camPos,Vector3 targetPos,Vector3 up){
		Vector2 result = new Vector2();
		Vector3 pos = targetPos-camPos;
		Quaternion upRotate = RotateFromUpVector(up);
		pos = Quaternion.Inverse(upRotate)*pos;
		result.x = Mathf.Rad2Deg*Mathf.Atan2(pos.x,pos.z);
		result.y = Vector3.Angle(Vector3.up,pos)-90;
		return result;
	}
	public void setUpVector(Vector3 upVector,bool isCameraStatic=false){
		playerUpVector=upVector;
		if(isCameraStatic){
			beforeUpVector=playerUpVector;
			Vector2 camRot = positionToCamRotate(CamTransform.position,transform.position,playerUpVector);
			rx=camRot.x;
			ry=camRot.y;
			Quaternion CamRotate = RotateFromUpVector(upVector)* Quaternion.AngleAxis(rx,Vector3.up)*Quaternion.AngleAxis(ry,Vector3.right);
			doStop1fCam=true;
			CamTransform.localRotation *= Quaternion.Inverse(CamRotate)*CamTransform.rotation;
		}
	}
	void minimizeCollider(bool isMinimize){
		if(isMinimize){
			characterCollider.height=0;
			characterCollider.center=new Vector3(0,characterCollider.radius-ccoliderHeight/2,0);
		}else{
			characterCollider.height=ccoliderHeight;
			characterCollider.center=Vector3.zero;
		}
	}
}
