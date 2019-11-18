using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMask : MonoBehaviour
{
    public Color color = new Color(0f, 0f, 0f, 0.5f);
    public bool closePanel = true;

    private RectTransform m_tranform;
    private Transform m_parentTransform;

    private void Awake()
    {
        m_tranform = GetComponent<RectTransform>();
        m_parentTransform = transform.parent;
    }

    private void Start()
    {
        if (m_parentTransform == null)
        {
            m_parentTransform = transform.parent;
            OnEnable();
        }
    }

    private GameObject mMaskGo = null;
    private void OnEnable()
    {
        if (mMaskGo == null && m_parentTransform != null)
        {
            DestroyGo();

            mMaskGo = new GameObject();
            var trans = mMaskGo.AddComponent<RectTransform>();
            trans.SetParent(m_parentTransform);
            trans.anchoredPosition3D = Vector2.zero;
            trans.sizeDelta = new Vector2(10000, 10000);
            trans.SetSiblingIndex(m_tranform.GetSiblingIndex());

            mMaskGo.AddComponent<Image>().color = color;
            mMaskGo.AddComponent<UIEventListener>().onClick.AddListener(OnClick);
        }
    }

    private void OnClick(Vector2 pos)
    {
        if (closePanel)
        {
            var p = transform;
            do
            {
                var panel = p.GetComponent<ViewBase>();
                if (panel != null)
                {
                    UIManager.Instance.Close(panel.GetType());
                    break;
                }
                p = p.parent;
            }
            while (p != null);
        }
    }

    private void DestroyGo()
    {
        if (mMaskGo != null)
        {
            Destroy(mMaskGo);
            mMaskGo = null;
        }
    }

    private void OnDisable()
    {
        DestroyGo();
    }
}
