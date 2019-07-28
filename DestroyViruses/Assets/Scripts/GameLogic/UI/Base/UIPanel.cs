using UnityEngine;
using System;

namespace DestroyViruses
{
    public abstract class UIPanel : MonoBehaviour
    {
        public abstract void OnInit();
        public abstract void OnOpen();
        public abstract void OnClose();
        public abstract void OnDestroy();
    }
}