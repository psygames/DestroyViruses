using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager : Singleton<UIManager>
{
    public RectTransform bottomLayer;
    public RectTransform commonLayer;
    public RectTransform topLayer;

    public Func<string, PanelBase> loadPanelFunc { get; set; }

    private readonly List<PanelBase> mCachedPanels = new List<PanelBase>();

    private PanelBase GetPanel(Type type)
    {
        foreach (var p in mCachedPanels)
        {
            if (p.GetType() == type)
            {
                return p;
            }
        }
        return null;
    }

    private void SetLayer(PanelBase panel, UILayer layer)
    {
        if (panel == null)
            return;

        Vector3 anchorPos3D = panel.rectTransform.anchoredPosition3D;
        Vector2 anchorPos = panel.rectTransform.anchoredPosition;
        Vector2 offsetMin = panel.rectTransform.offsetMin;
        Vector2 offsetMax = panel.rectTransform.offsetMax;

        if (layer == UILayer.Common && commonLayer != null)
        {
            panel.transform.SetParent(commonLayer);
        }
        else if (layer == UILayer.Top && topLayer != null)
        {
            panel.transform.SetParent(topLayer);
        }
        else if (layer == UILayer.Bottom && bottomLayer != null)
        {
            panel.transform.SetParent(bottomLayer);
        }
        panel.rectTransform.localScale = Vector3.one;
        panel.rectTransform.rotation = Quaternion.identity;
        panel.rectTransform.anchoredPosition3D = anchorPos3D;
        panel.rectTransform.anchoredPosition = anchorPos;
        panel.rectTransform.offsetMin = offsetMin;
        panel.rectTransform.offsetMax = offsetMax;
    }

    public PanelBase Load(Type panelType)
    {
        if (!typeof(PanelBase).IsAssignableFrom(panelType))
        {
            Debug.LogError($"{panelType.Name} is not a UIPanel");
            return null;
        }

        var panel = GetPanel(panelType);
        if (panel != null)
        {
            return panel;
        }

        if (loadPanelFunc == null)
        {
            Debug.LogError("UIManager Must set panelLoadFunc");
            return null;
        }

        panel = loadPanelFunc?.Invoke(panelType.Name);
        panel = Instantiate(panel);
        panel.gameObject.SetActive(false);
        mCachedPanels.Add(panel);
        return panel;
    }

    public T Load<T>() where T : PanelBase
    {
        return Load(typeof(T)) as T;
    }

    public T Open<T>(UILayer layer = UILayer.Common) where T : PanelBase
    {
        PanelBase panel = GetPanel(typeof(T));

        if (panel == null)
        {
            panel = Load<T>();
        }

        panel.gameObject.SetActive(true);
        SetLayer(panel, layer);
        panel.Invoke("OnOpen", 0f);
        return panel as T;
    }

    public void Close<T>() where T : PanelBase
    {
        PanelBase panel = GetPanel(typeof(T));
        panel.gameObject.SetActive(false);
        panel.Invoke("OnClose", 0);
    }

    public void Close(Type type)
    {
        if (!typeof(PanelBase).IsAssignableFrom(type))
        {
            Debug.LogError($"{type.Name} is not a UIPanel based Type");
            return;
        }
        PanelBase panel = GetPanel(type);
        panel.gameObject.SetActive(false);
        panel.Invoke("OnClose", 0);
    }
}
