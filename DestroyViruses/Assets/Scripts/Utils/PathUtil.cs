using UnityEngine;
using System;
using UnityEngine.UI;

namespace DestroyViruses
{
    public static class PathUtil
    {
        public const string ENTITY_PREFAB_ROOT = "Prefabs/Entities/";
        public static string GetEntity(string entitiName)
        {
            return ENTITY_PREFAB_ROOT + entitiName;
        }
    }
}