using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DestroyViruses
{
    public static class GameUtil
    {
        public static bool isFrozen
        {
            get
            {
                return GameModeManager.Instance.currentMode == null
                    || !GameModeManager.Instance.currentMode.isInit
                    || !GameModeManager.Instance.currentMode.isBegin
                    || GameModeManager.Instance.currentMode.isPause;
            }
        }
    }

    public static class LT
    {
        public static TableLanguage table
        {
            get { return TableLanguage.Get(Option.language); }
        }

        public static string systemLanguage
        {
            get
            {
                if (Application.systemLanguage == SystemLanguage.Chinese
                || Application.systemLanguage == SystemLanguage.ChineseSimplified
                || Application.systemLanguage == SystemLanguage.ChineseTraditional)
                    return "CH";
                else if (Application.systemLanguage == SystemLanguage.Spanish)
                    return "SP";
                else if (Application.systemLanguage == SystemLanguage.Russian)
                    return "RU";
                else if (Application.systemLanguage == SystemLanguage.Korean)
                    return "KO";
                else if (Application.systemLanguage == SystemLanguage.Japanese)
                    return "JA";
                else if (Application.systemLanguage == SystemLanguage.French)
                    return "FR";
                else if (Application.systemLanguage == SystemLanguage.German)
                    return "GR";
                else return "EN";
            }
        }
    }
}
