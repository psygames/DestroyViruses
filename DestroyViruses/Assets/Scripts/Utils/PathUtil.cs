using UnityEngine;
using System;
using UnityEngine.UI;

namespace DestroyViruses
{
    public static class PathUtil
    {
        private const string ASSET_ROOT = "";

        private const string ENTITY_PREFAB_ROOT = ASSET_ROOT + "Prefabs/Entities/";
        public static string Entity(string entitiName)
        {
            return ENTITY_PREFAB_ROOT + entitiName;
        }

        private const string UI_PANEL_PREFAB_ROOT = ASSET_ROOT + "Prefabs/UI/View/";
        public static string Panel(string panelName)
        {
            return UI_PANEL_PREFAB_ROOT + panelName;
        }

        private const string TABLE_ROOT = ASSET_ROOT + "Tables/";
        public static string Table(string name)
        {
            return TABLE_ROOT + name;
        }

        private const string SPRITE_ATLAS_ROOT = ASSET_ROOT + "SpriteAtlas/";
        public static string SpriteAtlas(string name)
        {
            return SPRITE_ATLAS_ROOT + name;
        }

        private const string SOUND_ROOT = ASSET_ROOT + "Sounds/";
        public static string Sound(string name)
        {
            return SOUND_ROOT + name;
        }

        private const string TEXTURE_ROOT = ASSET_ROOT + "Textures/";
        public static string Texture(string name)
        {
            return TEXTURE_ROOT + name;
        }

        public static string[] GetSpriteAtlasNames()
        {
            return new string[] { "total" };
        }
    }
}