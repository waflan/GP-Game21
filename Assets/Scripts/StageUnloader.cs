using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageUnloader : MonoBehaviour
{
    public StageLoader loader;
    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> loaders=new List<GameObject>();
        loaders.AddRange(GameObject.FindGameObjectsWithTag("Loader"));
        GameObject loaderObj = loaders.Find(gameObject=>{
            if (gameObject.GetComponent<StageLoader>()){
                return gameObject.GetComponent<StageLoader>().sceneName==this.gameObject.scene.name;
            }
            return false;
        });
        if(loaderObj!=null){
            loader=loaderObj.GetComponent<StageLoader>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerExit(Collider collision){
        if(collision.transform.tag=="MappingObj"){
            // Debug.Log(this.gameObject.scene.name);
            if(loader!=null){
                loader.active=true;
            }
            StartCoroutine(unloadScene());
        }
    }
    IEnumerator unloadScene()
    {
        yield return SceneManager.UnloadSceneAsync(this.gameObject.scene);
        // Debug.Log("after UnloadSceneAsync");
    }
}
