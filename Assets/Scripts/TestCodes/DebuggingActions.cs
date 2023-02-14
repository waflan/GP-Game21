using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebuggingActions : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    PlayerControl player;
    public Transform test;
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
        string result="";

		Quaternion UpVectorRotate = Quaternion.AngleAxis(Mathf.Rad2Deg*Mathf.Atan2(player.playerUpVector.x,player.playerUpVector.z),Vector3.up)*Quaternion.AngleAxis(Vector3.Angle(Vector3.up,player.playerUpVector),Vector3.right);
        result+=UpVectorRotate.ToString();
        test.rotation=UpVectorRotate;

        textMeshPro.text=result;
    }
}
