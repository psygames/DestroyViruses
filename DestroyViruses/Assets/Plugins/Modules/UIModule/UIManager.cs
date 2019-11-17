using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager : Singleton<UIManager>
{
    public RectTransform bottomLayer;
    public RectTransform commonLayer;
    public RectTransform topLayer;

    public Func<string, ViewBase> loadViewFunc { get; set; }

    private readonly List<ViewBase> mCachedViews = new List<ViewBase>();

    private ViewBase GetView(Type type)
    {
        foreach (var p in mCachedViews)
        {
            if (p.GetType() == type)
            {
                return p;
            }
        }
        return null;
    }

    private void SetLayer(ViewBase view, UILayer layer)
    {
        if (view == null)
            return;

        Vector3 anchorPos3D = view.rectTransform.anchoredPosition3D;
        Vector2 anchorPos = view.rectTransform.anchoredPosition;
        Vector2 offsetMin = view.rectTransform.offsetMin;
        Vector2 offsetMax = view.rectTransform.offsetMax;

        if (layer == UILayer.Common && commonLayer != null)
        {
            view.transform.SetParent(commonLayer);
        }
        else if (layer == UILayer.Top && topLayer != null)
        {
            view.transform.SetParent(topLayer);
        }
        else if (layer == UILayer.Bottom && bottomLayer != null)
        {
            view.transform.SetParent(bottomLayer);
        }
        view.rectTransform.localScale = Vector3.one;
        view.rectTransform.rotation = Quaternion.identity;
        view.rectTransform.anchoredPosition3D = anchorPos3D;
        view.rectTransform.anchoredPosition = anchorPos;
        view.rectTransform.offsetMin = offsetMin;
        view.rectTransform.offsetMax = offsetMax;
    }

    public ViewBase Load(Type viewType)
    {
        if (!typeof(ViewBase).IsAssignableFrom(viewType))
        {
            Debug.LogError($"{viewType.Name} is not a UIView");
            return null;
        }

        var view = GetView(viewType);
        if (view != null)
        {
            return view;
        }

        if (loadViewFunc == null)
        {
            Debug.LogError("UIManager Must set loadViewFunc");
            return null;
        }

        view = loadViewFunc?.Invoke(viewType.Name);
        view = Instantiate(view);
        view.gameObject.SetActive(false);
        mCachedViews.Add(view);
        return view;
    }

    public T Load<T>() where T : ViewBase
    {
        return Load(typeof(T)) as T;
    }

    public T Open<T>(UILayer layer = UILayer.Common) where T : ViewBase
    {
        ViewBase view = GetView(typeof(T));

        if (view == null)
        {
            view = Load<T>();
        }

        view.gameObject.SetActive(true);
        SetLayer(view, layer);
        view.Invoke("OnOpen", 0f);
        return view as T;
    }

    public void Close<T>() where T : ViewBase
    {
        ViewBase view = GetView(typeof(T));
        view.gameObject.SetActive(false);
        view.Invoke("OnClose", 0);
    }

    public void Close(Type type)
    {
        if (!typeof(ViewBase).IsAssignableFrom(type))
        {
            Debug.LogError($"{type.Name} is not a UIView based Type");
            return;
        }
        ViewBase view = GetView(type);
        view.gameObject.SetActive(false);
        view.Invoke("OnClose", 0);
    }
}
