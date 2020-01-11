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
        public Slider energyFill;
        public GameObject energyTipsObj;
        public Text energyTips;
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
            energyText.text = $"{D.I.energy}/{D.I.maxEnergy}";
            energyFill.value = 1f * D.I.energy / D.I.maxEnergy;
            diamondText.text = D.I.diamond.KMB();
        }

        public static void BlackSetting(bool isOn)
        {
            if (sIns != null)
                sIns.blackSetting.SetActive(isOn);
        }

        private void Update()
        {
            if (D.I.isEnergyMax)
            {
                energyTipsObj.SetActive(false);
                return;
            }

            energyTipsObj.SetActive(true);
            energyTips.text = LTKey.ENERGY_TIPS_X.LT(D.I.energyRechargeRemain);
        }
    }
}