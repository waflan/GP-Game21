using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMoveTest : MonoBehaviour
{
    KeyConfig keyConfig=new KeyConfig();

    public float speed = 10;
    public Vector2 camDistanceRange=new Vector2(1,10);
    public Vector2 rotateSpeed=new Vector2(270,180); // カメラ回転速度
    public Vector2 mouseRotateSpeed=new Vector2(5,5); // マウスでのカメラ回転速度

    public Vector3 playerVectorUp;
    Vector3 beforeUpVector;
    public Transform CamTransform;
    public  Transform RotateCenter;

    public float distance;

    float rx,ry,camDist;
    Rigidbody rig;
    // Start is called before the first frame update
    void Start()
    {
        rig=GetComponent<Rigidbody>();
        beforeUpVector=playerVectorUp;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 toCenter = (this.transform.position-RotateCenter.position).normalized;
        playerVectorUp=toCenter;

        Vector2 move;
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
        bool isMove=(move!=Vector2.zero);

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
        rMove.x+=Input.GetAxis("Mouse X")*mouseRotateSpeed.x;
        rMove.y-=Input.GetAxis("Mouse Y")*mouseRotateSpeed.y;

        rx+=rMove.x;
        ry+=rMove.y;
        ry=Mathf.Clamp(ry,-90,90);
        rx=Mathf.Repeat(rx+180,360)-180;

        // 上方ベクトルへの回転
        Quaternion UpVectorRotate = Quaternion.AngleAxis(Mathf.Rad2Deg*Mathf.Atan2(playerVectorUp.x,playerVectorUp.z),Vector3.up)*Quaternion.AngleAxis(Vector3.Angle(Vector3.up,playerVectorUp),Vector3.right);
        
        // 軸が更新されたときにカメラ回転の値を更新
        if(beforeUpVector!=playerVectorUp){
            if(playerVectorUp.y>0){
                rx-=Mathf.Rad2Deg*Mathf.Atan2(playerVectorUp.x,playerVectorUp.z)-Mathf.Rad2Deg*Mathf.Atan2(beforeUpVector.x,beforeUpVector.z);
            }else{
                rx+=Mathf.Rad2Deg*Mathf.Atan2(playerVectorUp.x,playerVectorUp.z)-Mathf.Rad2Deg*Mathf.Atan2(beforeUpVector.x,beforeUpVector.z);
            }
            float changeAngle = Vector3.Angle(playerVectorUp,beforeUpVector)*Mathf.Deg2Rad;
            Vector3 deltaVel = rig.velocity;
            rig.velocity = Quaternion.FromToRotation(beforeUpVector,playerVectorUp)*rig.velocity;
            deltaVel = rig.velocity-deltaVel;
            Debug.DrawRay(transform.position,deltaVel);
            Debug.DrawRay(transform.position,playerVectorUp,Color.red);
            // this.transform.position-=playerVectorUp*(rig.velocity.magnitude*Mathf.Sin(changeAngle)*0.05f);
            distance = (transform.position-RotateCenter.position).magnitude;
            beforeUpVector=playerVectorUp;
        }

        // 指定ベクトル基準で回転
        Quaternion CamRotate = UpVectorRotate* Quaternion.AngleAxis(rx,Vector3.up)*Quaternion.AngleAxis(ry,Vector3.right);
        Quaternion toRotate = UpVectorRotate*Quaternion.AngleAxis(rx+Mathf.Rad2Deg*Mathf.Atan2(move.x,move.y),Vector3.up);
        CamTransform.rotation = CamRotate;

        // カメラをプレイヤー後方に移動させる
        Ray ray= new Ray(this.transform.position,CamRotate*Vector3.back);
        RaycastHit hit= new RaycastHit();
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
        CamTransform.position=this.transform.position+CamRotate*(Vector3.back*camDist);

        if(isMove){
            transform.rotation=toRotate;
        }

        Vector3 movingVelocity = toRotate*Vector3.forward*speed;
        if(isMove){
            rig.velocity=movingVelocity;
        }else{
            rig.velocity=Vector3.zero;
        }
        
    }
}
