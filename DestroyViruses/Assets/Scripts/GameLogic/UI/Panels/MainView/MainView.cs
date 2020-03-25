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

        public GameObject dailySignObj;
        public GameObject dailySignReddotObj;
        public GameObject bookObj;
        public GameObject bookReddotObj;

        public GameObject trialTag;

        private void Awake()
        {
            ButtonListenerInit();
            SetMusic();
        }

        private void SetMusic()
        {
            AudioManager.MusicPlayer.mute = !Option.music;
            AudioManager.FireMusicPlayer.mute = !Option.sound;
            AudioManager.SoundPlayer.mute = !Option.sound;
        }

        private void OnEnable()
        {
            this.BindUntilDisable<EventGameData>(OnEventGameData);
            this.BindUntilDestroy<EventGameData>(OnEventGameDataWhenever);
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            RefreshUI();
            AudioManager.PlayMusic($"BGM{Random.Range(1, 3)}", 1f);
            NavigationView.BlackSetting(false);
            if (CheckVersion())
            {
                StartCoroutine(ShowOpenHints());
            }
        }

        bool needCheckVersion = true;
        bool CheckVersion()
        {
            if (!needCheckVersion)
                return true;

            needCheckVersion = false;
            var min = System.Version.Parse(GameLocalData.Instance.minVersion);
            var latest = System.Version.Parse(GameLocalData.Instance.latestVersion);
            var current = System.Version.Parse(Application.version);

            if (current < min || current < latest)
            {
                UIManager.Open<VersionUpdateView>(UILayer.Top);
                return false;
            }
            return true;
        }

        bool needOpenTutorial = false;
        bool needOpenRate = false;
        IEnumerator ShowOpenHints()
        {
            yield return new WaitForSeconds(1);
            foreach (var hint in mOpenHints)
            {
                Toast.Show(hint);
                yield return new WaitForSeconds(2);
            }

            mOpenHints.Clear();

            if (needOpenTutorial)
            {
                needOpenTutorial = false;
                TutorialView.Begin(baseOptionBtn.GetComponent<RectTransform>(), LTKey.TUTORIAL_CLICK_PANEL_BASE.LT(), 0, () =>
                {
                    TutorialView.Begin(UpgradeView.tutorialPowerUpBtn, LTKey.TUTORIAL_CLICK_UPDATE_POWER.LT(), 2);
                });
            }
            else if (needOpenRate)
            {
                needOpenRate = false;
                UIManager.Open<RateView>(UILayer.Top);
            }
            else if (D.I.CanDailySign())
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
                UIManager.Open<UpgradeView>();
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
            dailySignObj.SetActive(D.I.unlockedGameLevel >= CT.table.dailySignUnlockLevel);
            dailySignReddotObj.SetActive(D.I.CanDailySign());
            bookObj.SetActive(D.I.unlockedGameLevel >= CT.table.bookUnlockLevel);
            bookReddotObj.SetActive(bookObj.activeSelf && D.I.CanBookCollect());
            trialTag.SetActive(!D.I.noAd && D.I.HasTrialWeapon() && !D.I.IsInTrial());
        }

        List<string> mOpenHints = new List<string>();
        private void OnEventGameDataWhenever(EventGameData evt)
        {
            if (evt.action == EventGameData.Action.UnlockNewLevel)
            {
                if (D.I.unlockedGameLevel == 2)
                {
                    needOpenTutorial = true;
                }
                else if (!GameLocalData.Instance.isRateOver)
                {
                    foreach (var lv in CT.table.rateUsHintLevel)
                    {
                        if (lv == D.I.unlockedGameLevel - 1)
                        {
                            needOpenRate = true;
                            break;
                        }
                    }
                }

                if (D.I.unlockedGameLevel == CT.table.dailySignUnlockLevel)
                {
                    mOpenHints.Add(LTKey.UNLOCK_SYSTEM_X.LT(LTKey.DAILY_SIGN.LT()));
                }
                if (D.I.unlockedGameLevel == CT.table.bookUnlockLevel)
                {
                    mOpenHints.Add(LTKey.UNLOCK_SYSTEM_X.LT(LTKey.VIRUS_BOOK.LT()));
                }
                if (D.I.unlockedGameLevel == CT.table.weaponUnlockLevel)
                {
                    mOpenHints.Add(LTKey.UNLOCK_SYSTEM_X.LT(LTKey.WEAPON_SYSTEM.LT()));
                }
                foreach (var t in TableWeapon.GetAll())
                {
                    if (D.I.unlockedGameLevel == t.unlockLevel)
                    {
                        mOpenHints.Add(LTKey.UNLOCK_WEAPON_X.LT(LT.Get(t.nameID)));
                    }
                }
            }
        }

        private void OnEventGameData(EventGameData evt)
        {
            if (evt.action == EventGameData.Action.DataChange)
            {
                RefreshUI();
            }
            if (evt.action == EventGameData.Action.ChangeWeapon)
            {
                Aircraft.ins.weapon.Reset();
            }
        }


        private void OnClickSetting()
        {
            UIManager.Open<OptionView>(UILayer.Top);
        }

        private void OnClickDailySign()
        {
            UIManager.Open<DailySignView>(UILayer.Top);
        }

        private void OnClickBook()
        {
            UIManager.Open<BookView>();
        }

        private void OnClickTest()
        {
            gameLevelPanel.PlayLevelUp();
            this.DelayDo(1f, gameLevelPanel.SetData);
        }
    }
}