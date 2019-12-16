using UnityEngine;
using System;
using UnityEngine.UI;

namespace DestroyViruses
{
    public static class PathUtil
    {
        private const string ASSET_ROOT = "Assets/AssetBundles/";

        private const string ENTITY_PREFAB_ROOT = ASSET_ROOT + "Prefabs/Entities/";
        public static string Entity(string entitiName)
        {
            return ENTITY_PREFAB_ROOT + entitiName + ".prefab";
        }

        private const string UI_PANEL_PREFAB_ROOT = ASSET_ROOT + "Prefabs/UI/View/";
        public static string Panel(string panelName)
        {
            return UI_PANEL_PREFAB_ROOT + panelName + ".prefab";
        }

        private const string TABLE_ROOT = ASSET_ROOT + "Tables/";
        public static string Table(string name)
        {
            return TABLE_ROOT + name + ".bytes";
        }

        private const string SPRITE_ATLAS_ROOT = ASSET_ROOT + "SpriteAtlas/";
        public static string SpriteAtlas(string name)
        {
            return SPRITE_ATLAS_ROOT + name + ".spriteatlas";
        }

        private const string SOUND_ROOT = ASSET_ROOT + "Sounds/";
        public static string Sound(string name)
        {
            return "Sounds/" + name;
        }

        public static string[] GetSpriteAtlasNames()
        {
            return new string[] { "common", "sundry", "virus", "total" };
        }
    }
}