using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMoveTest : MonoBehaviour
{
    KeyConfig keyConfig;

    public AnimationCurve curve;

    public float speed = 10;
    public Vector2 camDistanceRange=new Vector2(1,10);
    public Vector2 rotateSpeed=new Vector2(270,180); // カメラ回転速度
    public Vector2 mouseRotateSpeed=new Vector2(5,5); // マウスでのカメラ回転速度

    public Vector3 playerUpVector;
    Vector3 beforeUpVector;
    public Transform CamTransform;
    public  Transform RotateCenter;

    public float rx,ry;
    float camDist;
    Rigidbody rig;
    // Start is called before the first frame update
    void Start()
    {
        rig=GetComponent<Rigidbody>();
        keyConfig=GetComponent<KeyConfig>();
        beforeUpVector=playerUpVector;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 toCenter = (this.transform.position-RotateCenter.position).normalized;
        playerUpVector=toCenter;

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
        Quaternion UpVectorRotate = RotateFromUpVector(playerUpVector);
        // Quaternion UpVectorRotate = Quaternion.AngleAxis(Mathf.Rad2Deg*Mathf.Atan2(playerUpVector.x,playerUpVector.z),Vector3.up)*Quaternion.AngleAxis(Vector3.Angle(Vector3.up,playerUpVector),Vector3.right);
        

        // Quaternion rot = Quaternion.AngleAxis(rx,Vector3.up);
        // Debug.Log($"{rot},{rx:F2},{Mathf.Atan2(rot.y,rot.w)*2*Mathf.Rad2Deg:F2}");

        // 軸が更新されたときにカメラ回転の値を更新
        if(beforeUpVector!=playerUpVector){

            Quaternion nextLocalRot = Quaternion.Inverse(UpVectorRotate)*RotateFromUpVector(beforeUpVector)*Quaternion.AngleAxis(rx,Vector3.up)*Quaternion.FromToRotation(beforeUpVector,playerUpVector);
            
// Mathf.Atan2(nextLocalRot.w,nextLocalRot.x)*Mathf.Rad2Deg
            // Quaternion rot = Quaternion.AngleAxis(rx,Vector3.up);
            // Debug.Log(Mathf.DeltaAngle(rx,Mathf.Atan2(rot.w,rot.y)*Mathf.Rad2Deg));
            Vector3 axis;
            float angle,deltaHolAngle;
            deltaHolAngle = (Mathf.Rad2Deg*Mathf.Atan2(playerUpVector.x,playerUpVector.z)-Mathf.Rad2Deg*Mathf.Atan2(beforeUpVector.x,beforeUpVector.z));
            deltaHolAngle = Mathf.Repeat(deltaHolAngle+180,360)-180;

            Quaternion.FromToRotation(beforeUpVector,playerUpVector).ToAngleAxis(out angle,out axis);
            Debug.DrawRay(RotateCenter.position,axis,Color.blue);
            angle = (90 - Mathf.Abs(Vector3.Angle(Vector3.up,axis)-90))*Mathf.Deg2Rad;

            // Debug.Log(angle);
            Debug.Log(deltaHolAngle);
            // Debug.Log($"{Mathf.Atan2(nextLocalRot.y,nextLocalRot.w)*2*Mathf.Rad2Deg:F2}:({angle:F2},{axis})");

            if(Input.GetKey(KeyCode.LeftShift)){
                float hoge = angle/Mathf.PI*2;
                hoge=curve.Evaluate(hoge);
                if(playerUpVector.y>0){
                    rx-=(deltaHolAngle)*(hoge);
                }else{
                    rx+=(deltaHolAngle)*(hoge);
                }
                // nextLocalRot.ToAngleAxis(out angle,out axis);
                // rx = (axis.y>0?1:-1)*angle;
            }else{
                
                if(playerUpVector.y>0){
                    rx-=Mathf.Rad2Deg*Mathf.Atan2(playerUpVector.x,playerUpVector.z)-Mathf.Rad2Deg*Mathf.Atan2(beforeUpVector.x,beforeUpVector.z);
                }else{
                    rx+=Mathf.Rad2Deg*Mathf.Atan2(playerUpVector.x,playerUpVector.z)-Mathf.Rad2Deg*Mathf.Atan2(beforeUpVector.x,beforeUpVector.z);
                }
            }
            



            // Debug.Log($"{Mathf.DeltaAngle(rx,Mathf.Atan2(nextLocalRot.y,nextLocalRot.w)*2*Mathf.Rad2Deg):F1}");

            float changeAngle = Vector3.Angle(playerUpVector,beforeUpVector)*Mathf.Deg2Rad;
            
            // this.transform.position-=playerUpVector*(rig.velocity.magnitude*Mathf.Sin(changeAngle)*0.05f);
            beforeUpVector=playerUpVector;

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

        // 動いた方向に向ける
        if(isMove){
            transform.rotation=toRotate;
        }

        Vector3 movingVelocity = toRotate*Vector3.forward*speed;
        if(isMove){
            rig.velocity=movingVelocity;
        }else{
            rig.velocity=Vector3.zero;
        }
        transform.position=transform.position.normalized*1.1f;
    }
    Quaternion RotateFromUpVector(Vector3 up){
        return Quaternion.AngleAxis(Mathf.Rad2Deg*Mathf.Atan2(up.x,up.z),Vector3.up)*Quaternion.AngleAxis(Vector3.Angle(Vector3.up,up),Vector3.right);
    }
}
