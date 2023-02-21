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
    public Vector3 beforeUpVector;
    public Transform CamTransform;
    public  Transform RotateCenter;

    public float rx,ry;
    float befMoveRot=0;
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
        float moveRotate;
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
        moveRotate=isMove?(Mathf.Rad2Deg*Mathf.Atan2(move.x,move.y)):0;
        

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

        if(Input.GetKeyDown(KeyCode.Space)){
            UnityEditor.EditorApplication.isPaused = true;
        }

        // 上方ベクトルへの回転
        Quaternion UpVectorRotate = RotateFromUpVector(playerUpVector);
        // Quaternion UpVectorRotate = Quaternion.AngleAxis(Mathf.Rad2Deg*Mathf.Atan2(playerUpVector.x,playerUpVector.z),Vector3.up)*Quaternion.AngleAxis(Vector3.Angle(Vector3.up,playerUpVector),Vector3.right);
        

        // Quaternion rot = Quaternion.AngleAxis(rx,Vector3.up);
        // Debug.Log($"{rot},{rx:F2},{Mathf.Atan2(rot.y,rot.w)*2*Mathf.Rad2Deg:F2}");


        

        // 軸が更新されたときにカメラ回転の値を更新
        if(beforeUpVector!=playerUpVector){

            rx = nextLocalRot(rx+befMoveRot,playerUpVector,beforeUpVector,true)-befMoveRot;

            beforeUpVector=playerUpVector;
        }
        befMoveRot=moveRotate;
        DrawMoveFronts(rx,0,30);
        DrawMoveFronts(rx,90,30);
        DrawMoveFronts(rx,180,30);
        DrawMoveFronts(rx,-90,30);


        // 指定ベクトル基準で回転
        Quaternion CamRotate = UpVectorRotate*Quaternion.AngleAxis(rx,Vector3.up)*Quaternion.AngleAxis(ry,Vector3.right);
        Quaternion toRotate = UpVectorRotate*Quaternion.AngleAxis(rx+moveRotate,Vector3.up);
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
        transform.position=(transform.position-RotateCenter.position).normalized*1.1f;
    }
    Quaternion RotateFromUpVector(Vector3 up){
        return Quaternion.AngleAxis(Mathf.Rad2Deg*Mathf.Atan2(up.x,up.z),Vector3.up)*Quaternion.AngleAxis(Vector3.Angle(Vector3.up,up),Vector3.right);
    }

    void DrawMoveFronts(float front,float moveRotate,int count,float moveDist=0.1f){
        Vector3 pos=transform.position,befpos=pos;
        Vector3 up1=playerUpVector,up2=up1;
        float r=front;
        for (int i = 0; i < count; i++)
        {
            up1=(pos-RotateCenter.position).normalized;

            if(up1!=up2){
                r=nextLocalRot(r+moveRotate,up1,up2)-moveRotate;
            }

            Quaternion toRotate = RotateFromUpVector(up1)*Quaternion.AngleAxis(r+moveRotate,Vector3.up);
            befpos=pos;
            pos=(pos+(toRotate*Vector3.forward*moveDist)-RotateCenter.position).normalized*1.1f;
            
            up2=up1;

            Debug.DrawLine(pos,befpos,Color.HSVToRGB((float)i/count,1,1));
        }
    }

    float nextLocalRot(float localRot,Vector3 nowUp,Vector3 befUp,bool log=false){
        float result=Mathf.Repeat(localRot+180,360)-180;

        // Vector3 axis;
        // float angle,deltaHolAngle;
        // deltaHolAngle = (Mathf.Rad2Deg*Mathf.Atan2(nowUp.x,nowUp.z)-Mathf.Rad2Deg*Mathf.Atan2(befUp.x,befUp.z));
        // deltaHolAngle = Mathf.Repeat(deltaHolAngle+180,360)-180;
        // Quaternion.FromToRotation(befUp,nowUp).ToAngleAxis(out angle,out axis);
        // angle = (90 - Mathf.Abs(Vector3.Angle(Vector3.up,axis)-90))*Mathf.Deg2Rad;

        // float hoge = angle/Mathf.PI*2;
        // hoge=curve.Evaluate(hoge);
        // if(nowUp.y>0){
        //     result-=(deltaHolAngle)*(hoge);
        // }else{
        //     result+=(deltaHolAngle)*(hoge);
        // }


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
                    result=localRot-deltaHolAngle;
                }else{
                    result=localRot+deltaHolAngle;
                }
            }
            
            if(log){
                Debug.Log($"{dir1:F2}:{dir2:F2},rot:{localRot}→{result}");
            //     if(float.IsNaN(dir1)||float.IsNaN(dir2)){
                    Debug.Log($"up:{befUp}→{nowUp} lati:{lati1},{lati2}");
            //         UnityEditor.EditorApplication.isPaused = true;
            //     }
            }
            
        }
        

        return result;
    }
}
