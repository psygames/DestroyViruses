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
        public Button clearBtn;
        public InputField gameLevelInput;
        public Button selectBtn;
        public Button coinBtn;

        private void Awake()
        {
            clearBtn.OnClick(OnClear);
            selectBtn.OnClick(OnSelect);
            coinBtn.OnClick(OnCoin);
        }

        private void OnClear()
        {
            if (Directory.Exists(Application.persistentDataPath))
            {
                Directory.Delete(Application.persistentDataPath, true);
            }
            PlayerPrefs.DeleteAll();
            GameLocalData.Reload();
            Unibus.Dispatch(EventGameData.Get(EventGameData.Action.DataChange));
        }

        private void OnSelect()
        {
            if (int.TryParse(gameLevelInput.text, out var level))
            {
                GameLocalData.Instance.gameLevel = level;
                GameLocalData.Instance.unlockedGameLevel = level;
                GameLocalData.Instance.Save();
                Unibus.Dispatch(EventGameData.Get(EventGameData.Action.DataChange));
            }
        }

        private void OnCoin()
        {
            GameLocalData.Instance.coin = 99999999;
            GameLocalData.Instance.Save();
            Unibus.Dispatch(EventGameData.Get(EventGameData.Action.DataChange));
        }

        protected override void OnOpen()
        {
            gameLevelInput.text = GDM.ins.gameLevel.ToString();
        }
    }
}