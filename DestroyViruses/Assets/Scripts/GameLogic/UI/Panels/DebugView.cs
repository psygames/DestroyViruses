using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;
using System.IO;

namespace DestroyViruses
{
    public class DebugView : ViewBase
    {
        public InputField gameLevelInput;

        private void OnClickClear()
        {
            if (Directory.Exists(Application.persistentDataPath))
            {
                Directory.Delete(Application.persistentDataPath, true);
            }
            PlayerPrefs.DeleteAll();
            GameLocalData.Reload();
            BookData.Reload();
            WeaponLevelData.Reload();
            Unibus.Dispatch(EventGameData.Get(EventGameData.Action.DataChange));
        }

        private void OnClickSelect()
        {
            if (int.TryParse(gameLevelInput.text, out var level))
            {
                GameLocalData.Instance.gameLevel = level;
                GameLocalData.Instance.unlockedGameLevel = level;
                if (GameLocalData.Instance.weaponId == 0 && level >= ConstTable.table.weaponUnlockLevel)
                    GameLocalData.Instance.weaponId = 1;
                GameLocalData.Instance.Save();
                Unibus.Dispatch(EventGameData.Get(EventGameData.Action.DataChange));
            }
        }

        private void OnClickCollectCount()
        {
            if (int.TryParse(gameLevelInput.text, out var level))
            {
                for (int i = 0; i < 100; i++)
                {
                    BookData.Instance.Set(i, level);
                }
                BookData.Instance.Save();
                Unibus.Dispatch(EventGameData.Get(EventGameData.Action.DataChange));
            }
        }

        private void OnClickCoin()
        {
            GameLocalData.Instance.coin = 99999999;
            GameLocalData.Instance.Save();
            Unibus.Dispatch(EventGameData.Get(EventGameData.Action.DataChange));
        }

        private void OnClickDiamond()
        {
            GameLocalData.Instance.diamond = 99999;
            GameLocalData.Instance.Save();
            Unibus.Dispatch(EventGameData.Get(EventGameData.Action.DataChange));
        }

        private void OnClickEnergy()
        {
            GameLocalData.Instance.energy = ConstTable.table.energyMax;
            GameLocalData.Instance.Save();
            Unibus.Dispatch(EventGameData.Get(EventGameData.Action.DataChange));
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            gameLevelInput.text = D.I.gameLevel.ToString();
        }
    }
}