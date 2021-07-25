using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrockBreakerMng : MonoBehaviour
{
    public bool isPlay=false;
    
    public Camera cam;
    RenderTexture renderTexture;
    public KeyConfig keyConfig=new KeyConfig();
    public GameObject ballPrefub;
    public GameObject blockPrefub;
    public Transform bar;
    Rigidbody2D rigBar;

    public List<Transform> blocks=new List<Transform>();

    public Vector2 blocksSize=new Vector2(5,5);
    public int width=3,height=4;
    
    public int ballCnt=0;

    public int blockCnt=0;
    public float ballSpd=5;

    public float speed=5;
    
    public float padding=.1f;
    
    // Start is called before the first frame update
    void Awake(){
        cam.targetTexture=new RenderTexture(cam.targetTexture);
    }
    void Start()
    {
        rigBar = bar.GetComponent<Rigidbody2D>();
        generateBall();
        Vector3 pos=this.transform.position;
        float cx=-(float)(width-1)/width/2;
        float cy=.5f-(float)(height-1)/height/2;
        for(int i=0;i<width;i++){
            for(int j=0;j<height;j++){
                GameObject block = Instantiate(blockPrefub);
                block.GetComponent<BB_Block>().mng=this;
                block.transform.parent=this.transform;
                block.transform.position=pos+new Vector3((cx+(float)i/width)*blocksSize.x,(cy+(float)j/height)*blocksSize.y,0);
                block.transform.localScale=new Vector3(blocksSize.x/width-padding,blocksSize.y/height-padding,1);
                blocks.Add(block.transform);
            }
        }
        blockCnt=width*height;
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlay){
            float xMove=0;
            if(Input.GetKey(keyConfig.wsad[2])){
                xMove-=1;
            }
            if(Input.GetKey(keyConfig.wsad[3])){
                xMove+=1;
            }
            rigBar.velocity=new Vector2(xMove*speed,0);

            if(ballCnt==0){
                blocks.ForEach(t=>t.gameObject.SetActive(true));
                generateBall();
            }
        }
        
    }

    void generateBall(){
        GameObject ball = Instantiate(ballPrefub,bar.position,Quaternion.identity);
        ball.GetComponent<BB_Ball>().mng=this;
        ball.GetComponent<BB_Ball>().speed=ballSpd;
        float rot =Mathf.Deg2Rad * Random.Range(-30,30);
        ball.GetComponent<Rigidbody2D>().velocity=new Vector2(Mathf.Sin(rot),Mathf.Cos(rot))*ballSpd;
        ballCnt++;
    }
    public void setIsPlay(bool b){
        isPlay=b;
    }
}
