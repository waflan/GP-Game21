#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GetRenderTex))]
public class GetRenderTexEditor : Editor
{
    GetRenderTex targetClass;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUI.BeginChangeCheck();

        targetClass = target as GetRenderTex;
        targetClass.enableTexName = EditorGUILayout.BeginToggleGroup("テクスチャ先の指定", targetClass.enableTexName);
        EditorGUI.indentLevel++;
        if (targetClass.enableTexName){
            targetClass.textureName = EditorGUILayout.TextField("テクスチャ", targetClass.textureName);
        }else{
            EditorGUILayout.TextField("テクスチャ", "_MainTex");
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndToggleGroup();
        targetClass.enableUpdateTiles = EditorGUILayout.BeginToggleGroup("テクスチャタイリングの指定", targetClass.enableUpdateTiles);
        EditorGUI.indentLevel++;
        if(targetClass.enableUpdateTiles){
            targetClass.tiling = EditorGUILayout.Vector2Field("タイリング",targetClass.tiling);
            targetClass.offset = EditorGUILayout.Vector2Field("オフセット",targetClass.offset);
        }else{
            EditorGUILayout.Vector2Field("タイリング",Vector2.one);
            EditorGUILayout.Vector2Field("オフセット",Vector2.zero);
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndToggleGroup();


        targetClass.targetType = (GetRenderTex.TargetType)EditorGUILayout.EnumPopup("ソース", targetClass.targetType);
        EditorGUI.indentLevel++;
        switch (targetClass.targetType)
        {
            case GetRenderTex.TargetType.Camera:

                targetClass.cam = (Camera)EditorGUILayout.ObjectField("Camera", targetClass.cam, typeof(Camera), true);

                break;
            case GetRenderTex.TargetType.RenderTexture:

                targetClass.renderTex = (RenderTexture)EditorGUILayout.ObjectField("RenderTexture", targetClass.renderTex, typeof(RenderTexture), true);
                break;
            default:
                break;
        }
        EditorGUI.indentLevel--;

        

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RegisterCompleteObjectUndo(target, "Changed GetRenderTex");
        }

        // base.OnInspectorGUI();


        // spawnObjectID=GUILayout.TextField(spawnObjectID);

        // ObjectsUtility targetClass = target as ObjectsUtility;

        // if (GUILayout.Button("hoge"))
        // {   
        //     targetClass.spawnObject(spawnObjectID,targetClass.transform.position);
        // }
    }
}

#endif
