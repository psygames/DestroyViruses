using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniPool;
namespace DestroyViruses
{
    public class EntityBase : MonoBehaviour
    {
        public const string prefabPathRoot = "Prefabs/Entities/";

        public void Recycle()
        {
            PoolManager.ReleaseObject(gameObject);
        }
    }
}