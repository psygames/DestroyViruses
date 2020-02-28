using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class BattleView : ViewBase
    {
        public GameLevelPanel gameLevelPanel;

        public CanvasGroup progressCanvasGroup;
        public Slider progressFill;
        public Text progressText;
        public RectTransform coinTransform;
        public Text coinText;

        public GameObject bossWaveToast;
        public FadeAlpha slowDownFade;

        private bool mLastIsBattleTouchOn;
        private Vector2 mLevelPanelRawPos;
        private RectTransform mLevelPanelRect;

        private void Awake()
        {
            this.BindUntilDisable<EventGameProcedure>(OnEventGameProcedure);
            this.BindUntilDisable<EventBattle>(OnEventBattle);
            mLevelPanelRawPos = gameLevelPanel.GetComponent<RectTransform>().anchoredPosition;
            mLevelPanelRect = gameLevelPanel.GetComponent<RectTransform>();
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            Aircraft.ins.anima.PlayHomeOut();
            bossWaveToast.SetActive(false);
            slowDownFade.FadeInImmediately();
            AudioManager.PlayMusic($"BGM{Random.Range(3, 5)}", 0.6f);
            gameLevelPanel.SetData();
            mLastIsBattleTouchOn = GlobalData.isBattleTouchOn;
            if (mLastIsBattleTouchOn) slowDownFade.FadeOutImmediately();
            else slowDownFade.FadeInImmediately();
            mLevelPanelRect.anchoredPosition = mLevelPanelRawPos;
            mLevelPanelRect.localScale = Vector3.one * 0.5f;
            progressCanvasGroup.alpha = 1;
        }

        private void OnEventGameProcedure(EventGameProcedure procedure)
        {
            if (procedure.action == EventGameProcedure.Action.BossWave)
            {
                ToastBossWave();
            }
            else if (procedure.action == EventGameProcedure.Action.GameEndWin)
            {
                ShowGameEndPanel(true);
            }
            else if (procedure.action == EventGameProcedure.Action.GameEndLose)
            {
                ShowGameEndPanel(false);
            }
        }

        private void OnEventBattle(EventBattle evt)
        {
            if (evt.action == EventBattle.Action.GET_COIN)
            {
                Coin.CreateGroup(evt.position, coinTransform.GetUIPos(), evt.count);
                coinText.text = D.I.battleGetCoin.KMB();
            }
        }

        private void Update()
        {
            float progress = 1 - D.I.battleProgress;
            progressFill.value = progress;
            progressText.text = $"{LTKey.REMAIN_VIRUS_COUNT.LT()}{Mathf.CeilToInt(progress * 100)}%";

            if (mLastIsBattleTouchOn != GlobalData.isBattleTouchOn)
            {
                if (GlobalData.isBattleTouchOn) slowDownFade.FadeOut();
                else slowDownFade.FadeIn();
                mLastIsBattleTouchOn = GlobalData.isBattleTouchOn;
            }
        }

        private void ToastBossWave()
        {
            bossWaveToast.SetActive(true);
            Observable.Timer(5).Subscribe(_ =>
            {
                bossWaveToast.SetActive(false);
            });
            AudioManager.PlaySound("boss");
        }

        private void ShowGameEndPanel(bool isWin)
        {
            if (isWin)
            {
                this.DelayDo(0.5f, () =>
                {
                    mLevelPanelRect.DOAnchorPos(mLevelPanelRawPos + Vector2.down * 300, 0.25f);
                    mLevelPanelRect.DOScale(1, 0.25f);
                    progressCanvasGroup.DOFade(0, 0.25f);
                    ResAddEffect.Play(ResAddView.ResType.Energy, ConstTable.table.energyRecoverWin);
                });

                this.DelayDo(1f, () =>
                {
                    Aircraft.ins.anima.PlayFlyAway();
                });

                this.DelayDo(2f, () =>
                {
                    UIManager.Open<NavigationView>(UILayer.Top);
                    gameLevelPanel.PlayLevelUp();
                });

                this.DelayDo(2.5f, () =>
                {
                    UIManager.Open<GameEndView>();
                });
            }
            else
            {
                this.DelayDo(0.5f, () =>
                {
                    Aircraft.ins.anima.PlayCrash();
                });

                this.DelayDo(1f, () =>
                {
                    UIManager.Open<GameEndView>();
                });
            }
        }
    }
}