using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DestroyViruses
{
    public static class LT
    {
        public static readonly string[] Tags = { TAG_EN, TAG_CN };
        public const string TAG_DEFAULT = TAG_EN;
        public const string TAG_EN = "en";
        public const string TAG_CN = "cn";
        public const string TAG_FR = "fr";
        public const string TAG_SP = "sp";
        public const string TAG_DE = "de";
        public const string TAG_RU = "ru";
        public const string TAG_JA = "ja";
        public const string TAG_KO = "ko";

        public static string Get(string key, params object[] args)
        {
            var t = TableLanguage.Get(key);
            if (t == null)
            {
#if UNITY_EDITOR || !PUBLISH_BUILD
                return $"[{key}]";
#else
                return key;
#endif
            }

            var tag = Option.language;
            string val;
            if (tag == TAG_EN) val = t.en;
            else if (tag == TAG_CN) val = t.cn;
            else val = t.en;

            if (args == null || args.Length <= 0)
            {
                return val;
            }
            return string.Format(val, args);
        }

        public static string systemLanguage
        {
            get
            {
                if (Application.systemLanguage == SystemLanguage.Chinese
                || Application.systemLanguage == SystemLanguage.ChineseSimplified
                || Application.systemLanguage == SystemLanguage.ChineseTraditional)
                    return TAG_CN;
                else if (Application.systemLanguage == SystemLanguage.Spanish)
                    return TAG_SP;
                else if (Application.systemLanguage == SystemLanguage.Russian)
                    return TAG_RU;
                else if (Application.systemLanguage == SystemLanguage.Korean)
                    return TAG_KO;
                else if (Application.systemLanguage == SystemLanguage.Japanese)
                    return TAG_JA;
                else if (Application.systemLanguage == SystemLanguage.French)
                    return TAG_FR;
                else if (Application.systemLanguage == SystemLanguage.German)
                    return TAG_DE;
                else if (Application.systemLanguage == SystemLanguage.English)
                    return TAG_EN;
                else return TAG_DEFAULT;
            }
        }

    }

    public static class LTExtension
    {
        public static string LT(this string key)
        {
            return DestroyViruses.LT.Get(key);
        }

        public static string LT(this string key, params object[] args)
        {
            return DestroyViruses.LT.Get(key, args);
        }
    }
}
