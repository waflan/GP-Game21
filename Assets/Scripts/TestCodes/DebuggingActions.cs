using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebuggingActions : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    PlayerControl player;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
        if(textMeshPro==null){
            if((textMeshPro=transform.GetComponentInChildren<TextMeshProUGUI>())==null){
                GameObject child = new GameObject("Text");
                textMeshPro = child.AddComponent<TextMeshProUGUI>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        string textout = "";
        textout += player.isRoll + ":" + player.onGround + ":" + player.jump;
        textMeshPro.text = textout;
    }
}
