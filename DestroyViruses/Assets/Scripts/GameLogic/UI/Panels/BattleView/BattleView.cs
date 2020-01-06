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
        public RectTransform coinTransform;
        public Text coinText;
        public Slider progressFill;
        public Text progressText;

        public GameObject bossWaveToast;
        public FadeAlpha slowDownFade;

        private bool mLastIsBattleTouchOn;

        private void Awake()
        {
            this.BindUntilDisable<EventGameProcedure>(OnEventGameProcedure);
            this.BindUntilDisable<EventBattle>(OnEventBattle);
        }

        protected override void OnOpen()
        {
            Aircraft.ins.anima.PlayHomeOut();
            bossWaveToast.SetActive(false);
            slowDownFade.FadeInImmediately();
            AudioManager.Instance.PlayMusic($"BGM{Random.Range(3, 5)}", 0.6f);
            gameLevelPanel.SetData();
            mLastIsBattleTouchOn = GlobalData.isBattleTouchOn;
            if(mLastIsBattleTouchOn) slowDownFade.FadeOutImmediately();
            else slowDownFade.FadeInImmediately();
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
            AudioManager.Instance.PlaySound("boss");
        }

        private void ShowGameEndPanel(bool isWin)
        {
            // TODO: game end
            UIManager.Open<GameEndView>(UILayer.Top);
        }
    }
}