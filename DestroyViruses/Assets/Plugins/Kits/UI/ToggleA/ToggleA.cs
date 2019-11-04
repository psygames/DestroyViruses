using UnityEngine;

/// <summary>
/// Simple toggle -- something that has an 'on' and 'off' states: checkbox, toggle button, radio button, etc.
/// </summary>
[AddComponentMenu("Project UI/ToggleA", 31)]
[RequireComponent(typeof(RectTransform))]
public class ToggleA : MonoBehaviour
{
    public ToggleAGroup group;
    public UIEventListener listener;
    public bool findGroupInParent = true;

    [HideInInspector]
    public bool isOn = false;
    public RadioBase raido;
    public RadioBase[] raidoExtend;

    private void Awake()
    {
        listener.onClick.AddListener(OnClick);

        if (findGroupInParent)
            group = GetComponentInParent<ToggleAGroup>();
    }

    void OnClick(Vector2 pos)
    {
        if (group == null)
        {
            isOn = !isOn;
            UpdateState();
        }
        else
        {
            group.NotifyToggle(this);
        }
    }

    public void UpdateState()
    {
        if (raido != null)
            raido.Radio(isOn);
        if (raidoExtend != null && raidoExtend.Length > 0)
        {
            foreach (var radio in raidoExtend)
            {
                if (radio != null)
                {
                    radio.Radio(isOn);
                }
            }
        }
    }
}
