using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
#if !UNITY_EDITOR && PUBLISH_BUILD
            debugBtn.gameObject.SetActive(false);
#endif
        }

        private int mDebugHit = 0;
        private float mLastDebugHit = 0;
        private void ButtonListenerInit()
        {
            settingBtn.OnClick(() => { UIManager.Open<OptionView>(UILayer.Top); });
            debugBtn.OnClick(() =>
            {
                if (Time.time - mLastDebugHit <= 0.5f)
                {
                    mDebugHit++;
                    if (mDebugHit >= 2)
                    {
                        UIManager.Open<DebugView>(UILayer.Top);
                    }
                }
                else
                {
                    mDebugHit = 0;
                }
                mLastDebugHit = Time.time;
            });
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
                // Debug.Log(evt.errorMsg);
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

        private void OnClickAddEnergy()
        {
            UIManager.Open<EnergyView>(UILayer.Top);
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
            energyTips.text = LTKey.ENERGY_TIPS_X.LT(TimeStr(D.I.energyRechargeRemain));
        }

        private string TimeStr(int seconds)
        {
            var secStr = seconds % 60 < 10 ? $"0{seconds % 60}" : $"{seconds % 60}";
            if (seconds >= 600)
                return $"{seconds / 60}:{secStr}";
            else if (seconds >= 60)
                return $"0{seconds / 60}:{secStr}";
            else
                return $"00:{secStr}";
        }
    }
}