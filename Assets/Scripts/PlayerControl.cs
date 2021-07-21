using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    KeyConfig keyConfig=new KeyConfig();
    public int controlMode=-1;
    public int playingMode;
    public bool actionable=true;
    public Transform CamTransForm;

    Rigidbody rig;

    public Animator animator = new Animator();
    public CapsuleCollider characterCollider=new CapsuleCollider();

    // デバッグ用変数
    
    

    // ここからアクション用変数

        // ローカル変数
    float rx=0,ry=0; // カメラ方向(横,縦)
    bool cursorLock=true; // 
    float camDist=1;
    Vector2 move =new Vector3();
    Vector3 movingVelocity=new Vector3();
    bool onGround,jump,isRoll=false,befRoll,isDive,isMove;
    float jumpTime,rollTime;
    Vector3 GroundVelocity=new Vector3();
    Vector3 beforeUpVector=new Vector3();
    List<SkinnedMeshRenderer> skins=new List<SkinnedMeshRenderer>();
    float ccoliderHeight;

        // パブリック変数
    public float speed=10; // 移動速度
    [Range (0,1)]
    public float footGripTime=.1f; // 地上での移動速度更新にかかる時間＆慣性速度の抵抗
        [Range (0,3)]
    public float airGripTime=.5f; // 空中での移動速度更新にかかる時間
    [Range (0,1)]
    public float rotateTime=.2f; // プレイヤーの操作による回転の時間
    public Vector2 rotateSpeed=new Vector2(270,180); // カメラ回転速度
    public Vector2 mouseRotateSpeed=new Vector2(5,5); // マウスでのカメラ回転速度
    public Vector2 camDistanceRange=new Vector2(1,10); // カメラの距離範囲
    public Vector3 playerVectorUp=Vector3.up; // プレイヤーの上方向(デフォルト：y軸方向)
    [Range (0.1f,5)]
    public float jumpMaxTime=1; // ジャンプの加速時間
    public float firstJumpForce=300; // ジャンプ初動の力
    public float secondJumpForce=200; // ジャンプ継続時の加速度
    public float rollForce=300; // 飛び込みの力
    public float diveForce=300; // 飛び込みの力
    

    void Start(){
        rig=transform.GetComponent<Rigidbody>();
        beforeUpVector=playerVectorUp;
        skins.AddRange(animator.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>());
        ccoliderHeight=characterCollider.height;
    }
    void Update(){

        if(actionable){
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
            // 回転情報の更新
                // 矢印キー回転
            if(Input.GetKey(KeyCode.RightArrow)){
                rx+=rotateSpeed.x*Time.deltaTime;
            }
            if(Input.GetKey(KeyCode.LeftArrow)){
                rx-=rotateSpeed.x*Time.deltaTime;
            }
            if(Input.GetKey(KeyCode.UpArrow)){
                ry-=rotateSpeed.y*Time.deltaTime;
            }
            if(Input.GetKey(KeyCode.DownArrow)){
                ry+=rotateSpeed.y*Time.deltaTime;
            }
                // マウス回転
            if(cursorLock){
                rx+=Input.GetAxis("Mouse X")*mouseRotateSpeed.x;
                ry-=Input.GetAxis("Mouse Y")*mouseRotateSpeed.y;
            }
            if(Mathf.Abs(ry)>90){
                ry=ry<0?-90:90;
            }
            if(Mathf.Abs(rx)>180){
                if(rx>0){
                    rx-=360;
                }else{
                    rx+=360;
                }
            }
            // 前転＆しゃがみ
            if(Input.GetKey(keyConfig.roll)){
                Vector3 horizontalVel=rig.velocity-(playerVectorUp.normalized*Vector3.Dot(playerVectorUp.normalized,rig.velocity));
                if(isMove){
                    if(Input.GetKeyDown(keyConfig.roll)){
                        isRoll=true;
                        rollTime=0.9f;
                    }
                }else{
                    
                }
            }
        }
        
        // 操作モード切り替え
        if(Input.GetKeyDown(keyConfig.modeChange)){
            controlMode=(controlMode+1)%2;
        }

        if(controlMode==-1){ // デバッグ用テスト
            
            // 指定ベクトル基準で回転
            Quaternion CamRotate = Quaternion.FromToRotation(Vector3.up,playerVectorUp)* Quaternion.AngleAxis(rx,Vector3.up)*Quaternion.AngleAxis(ry,Vector3.right);
            Quaternion toRotate = Quaternion.FromToRotation(Vector3.up,playerVectorUp)*Quaternion.AngleAxis(rx+Mathf.Rad2Deg*Mathf.Atan2(move.x,move.y),Vector3.up);
            CamTransForm.rotation = CamRotate;
            CamTransForm.position = this.transform.position+CamTransForm.rotation*(Vector3.back*5);
            this.transform.rotation =toRotate;
            if(move!=Vector2.zero){
                rig.velocity=toRotate*Vector3.forward*speed;
            }else{
                rig.velocity=Vector3.zero;
            }
            

        }
        else if(controlMode==0||controlMode==1){ // 一人称＆三人称視点操作

            // 軸が更新されたときにカメラ回転の値を更新
            if(beforeUpVector!=playerVectorUp){
                if(playerVectorUp.y>0){
                    rx-=Mathf.Rad2Deg*Mathf.Atan2(playerVectorUp.x,playerVectorUp.z)-Mathf.Rad2Deg*Mathf.Atan2(beforeUpVector.x,beforeUpVector.z);
                }else{
                    rx+=Mathf.Rad2Deg*Mathf.Atan2(playerVectorUp.x,playerVectorUp.z)-Mathf.Rad2Deg*Mathf.Atan2(beforeUpVector.x,beforeUpVector.z);
                }
                float changeAngle = Vector3.Angle(playerVectorUp,beforeUpVector)*Mathf.Deg2Rad;
                this.transform.position-=playerVectorUp*(rig.velocity.magnitude*Mathf.Sin(changeAngle)*0.05f);
                
                beforeUpVector=playerVectorUp;
            }

            // 指定ベクトル基準で回転
            Quaternion UpVectorRotate = Quaternion.AngleAxis(Mathf.Rad2Deg*Mathf.Atan2(playerVectorUp.x,playerVectorUp.z),Vector3.up)*Quaternion.AngleAxis(Vector3.Angle(Vector3.up,playerVectorUp),Vector3.right);
            Quaternion CamRotate = UpVectorRotate* Quaternion.AngleAxis(rx,Vector3.up)*Quaternion.AngleAxis(ry,Vector3.right);
            Quaternion toRotate = UpVectorRotate*Quaternion.AngleAxis(rx+Mathf.Rad2Deg*Mathf.Atan2(move.x,move.y),Vector3.up);
            CamTransForm.rotation = CamRotate;
            
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
                CamTransForm.position=this.transform.position+CamRotate*(Vector3.back*camDist);
            }else{ // 一人称視点の場合
                CamTransForm.position=this.transform.position;
                hideShowMesh(false);
            }
            

            // 移動方向に合わせてプレイヤーを回転する
            if(isMove){
                transform.rotation=Quaternion.Lerp(transform.rotation,toRotate,Time.deltaTime/rotateTime);
            }else{
                Quaternion fixRotate = Quaternion.FromToRotation(transform.rotation*Vector3.up,playerVectorUp);
                transform.rotation=fixRotate*transform.rotation;
            }
            if(onGround){
                rig.angularVelocity=Vector3.Lerp(rig.angularVelocity,Vector3.zero,Time.deltaTime/footGripTime);
            }

            // 移動方向に合わせプレイヤーの速度を更新
                // 操作による速度を除く
            Vector3 velocity = rig.velocity-movingVelocity;
                // 操作による速度を更新
            if(isMove){
                // movingVelocity=toRotate*Vector3.forward*speed;
                movingVelocity=Vector3.Lerp(movingVelocity,toRotate*Vector3.forward*speed,Time.deltaTime/(onGround?footGripTime:airGripTime));
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

            // ジャンプの処理
            if(onGround&&Input.GetKeyDown(keyConfig.jump)){
                jump=true;
                jumpTime=jumpMaxTime;
                onGround=false;
                rig.AddForce(playerVectorUp*firstJumpForce);
                // animator.SetBool("Jump",true);
            }
            if(jump){
                if(Input.GetKey(keyConfig.jump)){
                    if(jumpTime>=0){
                        rig.AddForce(playerVectorUp* secondJumpForce*Time.deltaTime);
                    }
                }else{
                    jumpTime=0;
                }
            }
            jumpTime-=Time.deltaTime;

            // 前転の処理
            if(isRoll){
                characterCollider.height=0;
                characterCollider.center=new Vector3(0,characterCollider.radius-ccoliderHeight/2,0);
                if(!befRoll){
                    animator.SetTrigger("Roll");
                    if(!onGround){
                        isDive=true;
                        rig.AddForce(toRotate*Vector3.forward*diveForce);
                    }else{
                        rig.AddForce(toRotate*Vector3.forward*rollForce);
                    }
                }
                if(rollTime<0){
                        isRoll=false;
                        characterCollider.height=ccoliderHeight;
                        characterCollider.center=Vector3.zero;
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
            }
            befRoll=isRoll;

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
                    animator.SetTrigger("Fall");
                }
            }
            animator.SetBool("Running",(move!=Vector2.zero));
            animator.SetBool("OnGround",onGround);

            // Debug.Log(Input.GetKey(keyConfig.jump)+" "+jump+" "+ jumpTime+" "+Vector3.Dot(playerVectorUp.normalized,rig.velocity));

        }
        
    }
    private void OnTriggerStay(Collider collision)
    {
        if(collision.isTrigger){

        }else{
            onGround=true;
            if(Vector3.Dot(playerVectorUp.normalized,rig.velocity)<=0.1){
                jump=false;
            }
            if(collision.GetComponent<Rigidbody>()){
                GroundVelocity = collision.GetComponent<Rigidbody>().velocity;
            }else{
                GroundVelocity=Vector3.zero;
            }
        }
        
    }
    private void OnTriggerExit(Collider collision)
    {
        if(collision.isTrigger){

        }else{
            onGround=false;
        }
    }
    void hideShowMesh(bool value){
        foreach (SkinnedMeshRenderer mesh in skins)
        {
            if(value){
                mesh.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.On;
            }else{
                mesh.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
        }
    }

}
