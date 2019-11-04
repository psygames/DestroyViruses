using UnityEditor;

[CustomEditor(typeof(ToggleA), true)]
[CanEditMultipleObjects]
public class ToggleAEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ToggleA toggle = target as ToggleA;

        bool isOn = EditorGUILayout.Toggle("IsOn", toggle.isOn);
        if (!toggle.isOn && isOn)
        {
            toggle.isOn = isOn;
            if (toggle.group != null)
                toggle.group.NotifyToggle(toggle);
            else
                toggle.UpdateState();
        }
    }
}
