using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ToggleAGroup), true)]
[CanEditMultipleObjects]
public class ToggleAGroupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var group = target as ToggleAGroup;
        var toggles = group.GetComponentsInChildren<ToggleA>();
        int toggleIndex = 0;
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                toggleIndex = i;
                break;
            }
        }

        var toggleName = toggleIndex < toggles.Length ? toggles[toggleIndex].name : "null";
        EditorGUILayout.LabelField("Toggle: " + toggleName);
        toggleIndex = EditorGUILayout.IntField("ToggleIndex", toggleIndex);
        EditorGUILayout.LabelField("Toggles");
        GUIStyle sty = new GUIStyle();
        sty.padding.left = 80;
        sty.padding.right = 30;
        EditorGUILayout.BeginHorizontal(sty);
        for (int i = 0; i < toggles.Length; i++)
        {
            if (EditorGUILayout.Toggle(i == toggleIndex))
                toggleIndex = i;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Separator();
        group.Toggle(toggleIndex);
    }
}
