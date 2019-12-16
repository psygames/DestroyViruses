using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace DestroyViruses.Editor
{
    [CustomEditor(typeof(ViewBase), true)]
    public class ViewBaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var view = target as ViewBase;
            string fakeName = "[FAKE IMAGE]";
            var img = view.transform.Find(fakeName);
            if (img == null)
            {
                if (GUILayout.Button("Show Fake Image"))
                {
                    var imgPath = "Assets/UIFakes/" + target.GetType().Name;
                    if (System.IO.File.Exists(imgPath + ".png"))
                        imgPath += ".png";
                    else if (System.IO.File.Exists(imgPath + ".jpg"))
                        imgPath += ".jpg";
                    else
                    {
                        Debug.LogError("No Image Found with ext .png or .jpg -> " + imgPath);
                        return;
                    }

                    GameObject go = new GameObject();
                    go.name = fakeName;
                    var rect = go.AddComponent<RectTransform>();
                    var image = go.AddComponent<RawImage>();
                    go.transform.SetParent(view.transform);
                    rect.anchoredPosition3D = Vector2.zero;
                    rect.localRotation = Quaternion.identity;
                    rect.localScale = Vector2.one;
                    rect.anchorMin = Vector2.zero;
                    rect.anchorMax = Vector2.one;
                    rect.offsetMin = Vector2.zero;
                    rect.offsetMax = Vector2.zero;
                    image.texture = AssetDatabase.LoadAssetAtPath<Texture>(imgPath);
                    image.color = new Color(1, 1, 1, 0.5f);
                }
            }
            else
            {
                if (GUILayout.Button("Hide Fake Image"))
                {
                    DestroyImmediate(img.gameObject);
                }
            }

        }
    }
}