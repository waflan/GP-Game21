using UnityEngine;
public class MyFoundation
{
    public static void setLayerAll(Transform transform,int layer){
        transform.gameObject.layer=layer;
        foreach (Transform child in transform)
        {
            setLayerAll(child,layer);
        }
    }
}