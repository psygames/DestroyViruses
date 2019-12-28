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

    private ViewBase getView(Type type)
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

    private void setLayer(ViewBase view, UILayer layer)
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

    private ViewBase load(Type viewType)
    {
        if (!typeof(ViewBase).IsAssignableFrom(viewType))
        {
            Debug.LogError($"{viewType.Name} is not a UIView");
            return null;
        }

        var view = getView(viewType);
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

    private T load<T>() where T : ViewBase
    {
        return load(typeof(T)) as T;
    }

    private T open<T>(UILayer layer = UILayer.Common) where T : ViewBase
    {
        ViewBase view = getView(typeof(T));

        if (view == null)
        {
            view = Load<T>();
        }

        view.gameObject.SetActive(true);
        setLayer(view, layer);
        view.Invoke("OnOpen", 0f);
        return view as T;
    }

    private void close<T>() where T : ViewBase
    {
        ViewBase view = getView(typeof(T));
        view.gameObject.SetActive(false);
        view.Invoke("OnClose", 0);
    }

    private void close(Type type)
    {
        if (!typeof(ViewBase).IsAssignableFrom(type))
        {
            Debug.LogError($"{type.Name} is not a UIView based Type");
            return;
        }
        ViewBase view = getView(type);
        view.gameObject.SetActive(false);
        view.Invoke("OnClose", 0);
    }


    #region API
    public static T Load<T>() where T : ViewBase
    {
        return Instance.load<T>();
    }

    public static T Open<T>(UILayer layer = UILayer.Common) where T : ViewBase
    {
        return Instance.open<T>(layer);
    }

    public static void Close<T>() where T : ViewBase
    {
        Instance.close<T>();
    }

    public static void Close(Type type)
    {
        Instance.close(type);
    }
    #endregion
}
