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
        public UIEventListener inputListenser;
        public GameLevelPanel gameLevelPanel;
        public RectTransform coinTransform;
        public Text coinText;
        public Slider progressFill;
        public Text progressText;

        public GameObject bossWaveToast;
        public FadeAlpha slowDownFade;


        private void Awake()
        {
            InputListenerInit();
            this.BindUntilDisable<EventGameProcedure>(OnEventGameProcedure);
            this.BindUntilDisable<EventBattle>(OnEventBattle);
        }

        protected override void OnOpen()
        {
            UIUtil.aircraftTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutQuad);
            bossWaveToast.SetActive(false);
            GlobalData.isBattleTouchOn = false;
            slowDownFade.FadeInImmediately();
            AudioManager.Instance.PlayMusic($"Sounds/BGM{Random.Range(3, 5)}", 0.6f);
            gameLevelPanel.SetData();
        }

        private void OnDestroy()
        {
            inputListenser.onDown.RemoveAllListeners();
            inputListenser.onUp.RemoveAllListeners();
            inputListenser.onDrag.RemoveAllListeners();
        }

        private void InputListenerInit()
        {
            inputListenser.onDown.AddListener((data) =>
            {
                InputManager.Instance.Push(new InputData(InputType.Down, UIUtil.FormatToVirtual(data)));
                GlobalData.isBattleTouchOn = true;
                slowDownFade.FadeOut();
            });
            inputListenser.onUp.AddListener((data) =>
            {
                InputManager.Instance.Push(new InputData(InputType.Up, UIUtil.FormatToVirtual(data)));
                GlobalData.isBattleTouchOn = false;
                slowDownFade.FadeIn();
            });
            inputListenser.onDrag.AddListener((data) =>
            {
                InputManager.Instance.Push(new InputData(InputType.Drag, UIUtil.FormatToVirtual(data)));
            });
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
            progressText.text = $"{LT.table.REMAIN_VIRUS_COUNT}{Mathf.CeilToInt(progress * 100)}%";
        }

        private void ToastBossWave()
        {
            bossWaveToast.SetActive(true);
            Observable.Timer(5).Subscribe(_ =>
            {
                bossWaveToast.SetActive(false);
            });
            AudioManager.Instance.PlaySound("Sounds/boss");
        }

        private void ShowGameEndPanel(bool isWin)
        {
            // TODO: game end
            UIManager.Instance.Open<GameEndView>(UILayer.Top);
        }
    }
}