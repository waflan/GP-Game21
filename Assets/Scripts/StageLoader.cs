using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageLoader : MonoBehaviour
{
    public bool active=true;
    public string sceneName;
    public bool loadInStart=false;

    // Start is called before the first frame update
    void Start()
    {
        active=true;
        if(loadInStart){
            Time.timeScale=0;
            active=false;
            StartCoroutine(loadSceneAndTimeStart());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider collision){
        if(collision.transform.tag=="MappingObj"&&active){
            // Debug.Log("enter!");
            active=false;
            StartCoroutine(loadScene());
        }
    }
    IEnumerator loadScene()
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
    IEnumerator loadSceneAndTimeStart()
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            Time.timeScale=1;
    }
}
