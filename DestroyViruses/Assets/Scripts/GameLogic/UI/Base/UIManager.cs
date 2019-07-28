using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class UIManager : Singleton<UIManager>
    {
        public UIPanel[] preloads;

        private void Start()
        {
            Preload();
        }

        private void Preload()
        {
            foreach (var p in preloads)
            {

            }
        }

        public void Load(Type panelType)
        {
            if (!typeof(UIPanel).IsAssignableFrom(panelType))
            {
                Debug.LogError($"{panelType.Name} is not a UIPanel");
                return;
            }


        }

        public void Load<T>() where T : UIPanel
        {

        }

        public void Open()
        {

        }

    }
}