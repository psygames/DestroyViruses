using System;
using UnityEngine;

namespace DestroyViruses
{
    public class Option
    {
        public static bool music { get { return GetOption("Music", true); } set { SetOption("Music", value); } }
        public static bool sound { get { return GetOption("Sound", true); } set { SetOption("Sound", value); } }
        public static bool vibration { get { return GetOption("Vibration", true); } set { SetOption("Vibration", value); } }
        public static string language { get { return GetOption("Language", LT.systemLanguage); } set { SetOption("Language", value); } }

        private static bool GetOption(string key, bool defaultValue)
        {
            return bool.Parse(PlayerPrefs.GetString(key, defaultValue.ToString()));
        }

        private static string GetOption(string key, string defaultValue)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        private static int GetOption(string key, int defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        private static float GetOption(string key, float defaultValue)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        private static void SetOption(string key, bool value)
        {
            PlayerPrefs.SetString(key, value.ToString());
        }

        private static void SetOption(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        private static void SetOption(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        private static void SetOption(string key, float value)
        {
            PlayerPrefs.GetFloat(key, value);
        }
    }
}
