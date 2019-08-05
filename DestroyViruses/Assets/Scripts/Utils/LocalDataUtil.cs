using UnityEngine;

namespace DestroyViruses
{
    public static class LocalDataUtil
    {
        private static GameData sGameData;
        public static GameData gameData
        {
            get
            {
                if (sGameData == null)
                {
                    sGameData = new GameData();
                    sGameData.Read();
                }
                return sGameData;
            }
        }
    }

    public class GameData
    {
        public int firePowerLevel;  //火力
        public int fireSpeedLevel;  //射速
        public int gameLevel;       //关卡

        public void Save()
        {
            PlayerPrefs.SetInt("firePowerLevel", firePowerLevel);
            PlayerPrefs.SetInt("fireSpeedLevel", fireSpeedLevel);
            PlayerPrefs.SetInt("gameLevel", gameLevel);
        }

        public void Read()
        {
            firePowerLevel = PlayerPrefs.GetInt("firePowerLevel", 1);
            fireSpeedLevel = PlayerPrefs.GetInt("fireSpeedLevel", 1);
            gameLevel = PlayerPrefs.GetInt("gameLevel", 1);
        }
    }
}
