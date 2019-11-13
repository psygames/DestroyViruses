using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class BattlePanel : PanelBase
    {
        public UIEventListener inputListenser;

        public Text previousLevelText;
        public Text currentLevelText;
        public Text nextLevelText;
        public GameObject previousBossTag;
        public GameObject currentBossTag;
        public GameObject nextBossTag;

        public RectTransform coinTransform;
        public Text coinText;
        public Image progressFill;
        public Text progressText;

        public GameObject bossWaveToast;
        public FadeAlpha slowDownFade;


        private void Awake()
        {
            InputListenerInit();
            this.BindUntilDisable<EventGameProcedure>(OnEventGameProcedure);
            this.BindUntilDisable<EventVirus>(OnEventVirus);
        }

        protected override void OnOpen()
        {
            UIUtil.aircraftTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutQuad);
            bossWaveToast.SetActive(false);
            GlobalData.isBattleTouchOn = false;
            slowDownFade.FadeInImmediately();
            AudioManager.Instance.PlayMusic($"Sounds/BGM{Random.Range(3, 5)}", 0.6f);
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
            if (procedure.action == EventGameProcedure.Action.FinalWave)
            {
                ToastFinalWave();
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

        private void OnEventVirus(EventVirus evt)
        {
            if (evt.action == EventVirus.Action.DEAD)
            {
                float coin = FormulaUtil.CoinConvert(evt.value);
                Coin.CreateGroup(evt.virus.rectTransform.anchoredPosition, coinTransform.GetUIPos(), coin);
            }
        }

        private void Update()
        {
            if (GDM.ins.gameLevel - 1 <= 0)
            {
                previousLevelText.text = "-";
                previousBossTag.SetActive(false);
            }
            else
            {
                previousLevelText.text = (GDM.ins.gameLevel - 1).ToString();
                previousBossTag.SetActive(TableGameLevel.Get(a => a.level == GDM.ins.gameLevel - 1).isBoss);
            }

            currentLevelText.text = GDM.ins.gameLevel.ToString();
            currentBossTag.SetActive(TableGameLevel.Get(a => a.level == GDM.ins.gameLevel).isBoss);

            if (TableGameLevel.Get(a => a.level == GDM.ins.gameLevel) == null)
            {
                nextLevelText.text = "-";
                nextBossTag.SetActive(false);
            }
            else
            {
                nextLevelText.text = (GDM.ins.gameLevel + 1).ToString();
                nextBossTag.SetActive(TableGameLevel.Get(a => a.level == GDM.ins.gameLevel).isBoss);
            }

            if (GDM.ins.gameLevel + 1 > GDM.ins.unlockedGameLevel)
            {
                nextLevelText.color = UIUtil.GRAY_COLOR;
            }
            else
            {
                nextLevelText.color = currentLevelText.color;
            }

            float progress = 1 - GDM.ins.battleProgress;
            progressFill.fillAmount = progress;
            progressText.text = $"剩余病毒:{(int)(progress * 100)}%";
            coinText.text = GDM.ins.battleGetCoin.KMB();
        }

        private void ToastFinalWave()
        {
            bossWaveToast.SetActive(true);
            Observable.Timer(5).Subscribe(_ =>
            {
                bossWaveToast.SetActive(false);
            });
        }

        private void ShowGameEndPanel(bool isWin)
        {
            // TODO: game end
            UIManager.Instance.Open<GameEndPanel>(UILayer.Top);
        }
    }
}