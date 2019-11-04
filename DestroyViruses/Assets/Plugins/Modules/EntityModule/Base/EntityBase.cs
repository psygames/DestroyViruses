using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EntityBase : MonoBehaviour
{
    public int uid { get; protected set; } = -1;
    public bool isAlive { get; protected set; } = false;

    private RectTransform mRectTransform = null;
    public RectTransform rectTransform
    {
        get
        {
            if (mRectTransform == null)
                mRectTransform = GetComponent<RectTransform>();
            return mRectTransform;
        }
    }

    public void Recycle()
    {
        EntityManager.Recycle(this);
    }
}

public class EntityBase<T> : EntityBase where T : EntityBase
{
    public static T Create()
    {
        return EntityManager.Create<T>();
    }
}