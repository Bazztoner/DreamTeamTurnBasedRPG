using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Snapshotter
{
    public static GameObject photographer;

    public static Texture2D TakeAssetPhoto(Camera cam)
    {
        var rt = new RenderTexture((int)cam.pixelWidth,
                                      (int)cam.pixelHeight, (int)cam.depth,
                                      RenderTextureFormat.ARGB32);
        RenderTexture.active = rt;
        cam.targetTexture = rt;

        cam.Render();
        Texture2D image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
        image.ReadPixels(cam.pixelRect, 0, 0);
        image.Apply();

        RenderTexture.active = null;
        cam.targetTexture = null;
        return image;
    }

    static GameObject GetPhotographer()
    {
        if (!photographer)
        {
            string[] allPaths = AssetDatabase.FindAssets("AssetPhotographer");
            foreach (var item in allPaths)
            {
                var temp = AssetDatabase.GUIDToAssetPath(item);
                return (GameObject)AssetDatabase.LoadAssetAtPath(temp, typeof(GameObject));
            }
            return null;
        }
        return photographer;
    }

    public static Texture2D TakePhoto(GameObject target)
    {
        var tempPos = new Vector3(5000, 5000, 5000);

        if (!photographer)
        {
            photographer = GetPhotographer();
            if (!photographer)
            { 
                Debug.Log("No existe Photographer");
                return null;
            }
        }

        var camParent = GameObject.Instantiate(photographer).GetComponent<ModelPhotographer>();

        var inst = GameObject.Instantiate(target);
        var cam = camParent.GetComponent<Camera>();
        inst.transform.position = tempPos;
        camParent.transform.SetParent(inst.transform);
        camParent.transform.localPosition = camParent.initialPosition;
        camParent.transform.localRotation = Quaternion.Euler(camParent.initialRotation);
        camParent.transform.localScale = camParent.initialScale;
        var ret = TakeAssetPhoto(cam);
        GameObject.DestroyImmediate(inst);
        return ret;
    }
}
