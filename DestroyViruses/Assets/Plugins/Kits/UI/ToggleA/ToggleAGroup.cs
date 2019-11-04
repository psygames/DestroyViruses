using System;
using UnityEngine;

[AddComponentMenu("Project UI/ToggleA Group", 32)]
[RequireComponent(typeof(RectTransform))]
public class ToggleAGroup : MonoBehaviour
{
    public int index { get; private set; }

    public Action<int> onSelect { get; set; }

    public void NotifyToggle(ToggleA toggle)
    {
        var toggles = GetComponentsInChildren<ToggleA>();
        int i = 0;
        foreach (var _toogle in toggles)
        {
            _toogle.isOn = _toogle == toggle;
            _toogle.UpdateState();
            if (_toogle.isOn)
                index = i;
            i++;
        }
        if (onSelect != null)
            onSelect.Invoke(index);
    }

    public void Toggle(int index)
    {
        var toggles = GetComponentsInChildren<ToggleA>();
        foreach (var _toogle in toggles)
        {
            _toogle.isOn = _toogle == toggles[index];
            _toogle.UpdateState();
        }
        this.index = index;
    }
}
