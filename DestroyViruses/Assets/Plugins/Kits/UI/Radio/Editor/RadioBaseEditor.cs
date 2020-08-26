using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RadioBase), true)]
public class RadioBaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var radio = (target as RadioBase);
        var count = radio.GetOptionCount();
        var contents = new string[count];
        for (int i = 0; i < count; i++)
        {
            contents[i] = i.ToString();
        }

        EditorGUILayout.Space();
        var newIndex = GUILayout.SelectionGrid(radio.index, contents, Mathf.Min(count, 5));
        if (newIndex != radio.index)
            radio.Radio(newIndex);
    }
}
