using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class UIManager : Singleton<UIManager>
    {
        public UIPanel[] preloads;
        public RectTransform bottomLayer;
        public RectTransform commonLayer;
        public RectTransform topLayer;

        private readonly List<UIPanel> mCachedPanels = new List<UIPanel>();

        private void Start()
        {
            Preload();
        }

        private void Preload()
        {
            foreach (var p in preloads)
            {
                Load(p.GetType());
            }
        }

        private UIPanel GetPanel(Type type)
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

        private void SetLayer(UIPanel panel, UILayer layer)
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

        public UIPanel Load(Type panelType)
        {
            if (!typeof(UIPanel).IsAssignableFrom(panelType))
            {
                Debug.LogError($"{panelType.Name} is not a UIPanel");
                return null;
            }

            var panel = GetPanel(panelType);
            if (panel != null)
            {
                return panel;
            }

            panel = ResourceUtil.Load<UIPanel>(PathUtil.GetPanel(panelType.Name));
            panel = Instantiate(panel);
            panel.gameObject.SetActive(false);
            panel.OnInit();
            mCachedPanels.Add(panel);
            return panel;
        }

        public T Load<T>() where T : UIPanel
        {
            return Load(typeof(T)) as T;
        }

        public T Open<T>(UILayer layer = UILayer.Common) where T : UIPanel
        {
            UIPanel panel = GetPanel(typeof(T));

            if (panel == null)
            {
                panel = Load<T>();
            }

            panel.gameObject.SetActive(true);
            SetLayer(panel, layer);
            panel.OnOpen();
            return panel as T;
        }

        public T Close<T>() where T : UIPanel
        {
            UIPanel panel = GetPanel(typeof(T));
            panel.gameObject.SetActive(false);
            panel.OnClose();
            return panel as T;
        }
    }
}