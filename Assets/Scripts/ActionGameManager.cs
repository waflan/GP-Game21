using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionGameManager : MonoBehaviour
{
    public PlayerControl pCtrl;
    Rigidbody playerRig;
    public GameObject mappingObject;
    public Rigidbody rig;
    void Awake(){
        SceneManager.LoadScene("Player", LoadSceneMode.Additive);
        // SceneManager.LoadScene("StageRoot", LoadSceneMode.Additive);
        
    }
    // Start is called before the first frame update
    void Start()
    {
        pCtrl=GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        playerRig=pCtrl.GetComponent<Rigidbody>();
        rig=mappingObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        mappingObject.transform.position=pCtrl.transform.position;
        rig.velocity=playerRig.velocity;
    }
}
