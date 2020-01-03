using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class MainView : ViewBase
    {
        // panels
        public GameLevelPanel gameLevelPanel;

        public Button baseOptionBtn;
        public Button aircraftOptionBtn;
        public Button coinOptionBtn;

        public GameObject canAircraftUp;
        public GameObject canWeaponUp;
        public GameObject canIncomeUp;

        private void Awake()
        {
            ButtonListenerInit();
            SetMusic();
        }

        private void SetMusic()
        {
            AudioManager.Instance.MusicPlayer.mute = !Option.music;
            AudioManager.Instance.FireMusicPlayer.mute = !Option.sound;
            AudioManager.Instance.SoundPlayer.mute = !Option.sound;
        }

        private void OnEnable()
        {
            this.BindUntilDisable<EventGameData>(OnEventGameData);
        }

        protected override void OnOpen()
        {
            RefreshUI();
            AudioManager.Instance.PlayMusic($"BGM{Random.Range(1, 3)}", 1f);
            NavigationView.BlackSetting(false);
            if (D.I.CanDailySign())
            {
                UIManager.Open<DailySignView>(UILayer.Top);
            }
        }

        private void ButtonListenerInit()
        {
            baseOptionBtn.OnClick(() => { SelectOption(0); });
            aircraftOptionBtn.OnClick(() => { SelectOption(1); });
            coinOptionBtn.OnClick(() => { SelectOption(2); });
        }

        private void SelectOption(int optionIndex)
        {
            if (optionIndex == 0)
            {
                UIManager.Open<UpgradeView>();
            }
            else if (optionIndex == 1)
            {
                UIManager.Open<BookView>();
            }
            else if (optionIndex == 2)
            {
                UIManager.Open<CoinView>();
            }
        }

        private void RefreshUI()
        {
            gameLevelPanel.SetData();
            canAircraftUp.SetActive(!D.I.isFireSpeedLevelMax && D.I.coin >= D.I.fireSpeedUpCost
                || !D.I.isFirePowerLevelMax && D.I.coin >= D.I.firePowerUpCost);
            canWeaponUp.SetActive(!D.I.isWeaponSpeedLevelMax && D.I.coin >= D.I.weaponSpeedUpCost
                || !D.I.isWeaponPowerLevelMax && D.I.coin >= D.I.weaponPowerUpCost);
            canIncomeUp.SetActive(!D.I.isCoinIncomeLevelMax && D.I.coin >= D.I.coinIncomeUpCost
                || !D.I.isCoinValueLevelMax && D.I.coin >= D.I.coinValueUpCost);
        }

        private void OnEventGameData(EventGameData evt)
        {
            if (evt.action == EventGameData.Action.DataChange)
            {
                RefreshUI();
            }
            if (evt.action == EventGameData.Action.ChangeWeapon)
            {
                Aircraft.ins.Reset();
            }
        }
    }
}