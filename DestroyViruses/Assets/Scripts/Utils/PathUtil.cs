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

        public const string UI_PANEL_PREFAB_ROOT = "Prefabs/UI/Panel/";
        public static string GetPanel(string panelName)
        {
            return UI_PANEL_PREFAB_ROOT + panelName;
        }

        public const string SPRITE_ATLAS_ROOT = "SpriteAtlas/";
    }
}