using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugStageFuncs : MonoBehaviour
{
    public void gotoMainScene(){
        SceneManager.LoadScene("ActionRootScene");
    }
    public void gotoDebugScene(){
        SceneManager.LoadScene("PlayerActionTest");
    }
}
