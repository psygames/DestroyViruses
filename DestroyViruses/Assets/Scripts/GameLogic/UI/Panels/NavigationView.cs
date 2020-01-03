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
    public class NavigationView : ViewBase
    {
        public GameObject blackSetting;
        // top menu
        public Text coinText;
        public Text energyText;
        public Text diamondText;
        public Button settingBtn;
        public Button debugBtn;

        private static NavigationView sIns = null;

        void Awake()
        {
            sIns = this;
            ButtonListenerInit();
        }

        private void ButtonListenerInit()
        {
            settingBtn.OnClick(() => { UIManager.Open<OptionView>(UILayer.Top); });
            debugBtn.OnClick(() => { UIManager.Open<DebugView>(UILayer.Top); });
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            this.BindUntilDisable<EventGameData>(OnEventGameData);
            RefreshUI();
        }

        private void OnEventGameData(EventGameData evt)
        {
            if (evt.action == EventGameData.Action.DataChange)
            {
                RefreshUI();
            }
            else if (evt.action == EventGameData.Action.Error)
            {
                Debug.Log(evt.errorMsg);
                Toast.Show(evt.errorMsg);
            }
        }

        private void OnClickAddCoin()
        {
            UIManager.Open<ExchangeView>(UILayer.Top);
        }

        private void OnClickAddDiamond()
        {
            UIManager.Open<CoinView>();
        }

        private void RefreshUI()
        {
            coinText.text = D.I.coin.KMB();
            energyText.text = "100";
            diamondText.text = D.I.diamond.KMB();
        }

        public static void BlackSetting(bool isOn)
        {
            if (sIns != null)
                sIns.blackSetting.SetActive(isOn);
        }
    }
}