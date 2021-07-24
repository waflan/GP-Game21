using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrockBreakerMng : MonoBehaviour
{
    public KeyConfig keyConfig=new KeyConfig();
    public GameObject ballPrefub;
    public GameObject blockPrefub;
    public Transform bar;
    Rigidbody2D rigBar;

    List<Transform> blocks=new List<Transform>();

    public Vector2 blocksSize=new Vector2(5,5);
    public int width=3,height=4;
    
    public int ballCnt=0;

    public int blockCnt=0;
    public float ballSpd=5;

    public float speed=5;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rigBar = bar.GetComponent<Rigidbody2D>();
        generateBall();
        Vector3 pos=this.transform.position;
        float cx=-(width%2==0?1.5f:1.0f)/width;
        float cy=-(height%2==0?1.5f:1.0f)/height;
        for(int i=0;i<width;i++){
            for(int j=0;j<1;j++){
                GameObject block = Instantiate(blockPrefub);
                block.transform.parent=this.transform;
                block.transform.position=pos+new Vector3((cx+(float)i/width)*blocksSize.x,0,0);
                blocks.Add(block.transform);
                Debug.Log((cx+i));
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        float xMove=0;
        if(Input.GetKey(keyConfig.wsad[2])){
            xMove-=1;
        }
        if(Input.GetKey(keyConfig.wsad[3])){
            xMove+=1;
        }
        rigBar.velocity=new Vector2(xMove*speed,0);
    }

    void generateBall(){
        GameObject ball = Instantiate(ballPrefub,bar.position,Quaternion.identity);
        ball.GetComponent<BB_Ball>().mng=this;
        float rot =Mathf.Deg2Rad * Random.Range(-30,30);
        ball.GetComponent<Rigidbody2D>().velocity=new Vector2(Mathf.Sin(rot),Mathf.Cos(rot))*ballSpd;
        ballCnt++;
    }
}
