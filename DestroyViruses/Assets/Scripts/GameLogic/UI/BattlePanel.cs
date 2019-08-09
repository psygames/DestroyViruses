using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class BattlePanel : UIPanel
    {
        public UIEventListener inputListenser;

        public override void OnInit()
        {
            InputListenerInit();
            this.BindUntilDisable<EventGameProcedure>(OnGameProcedure);
        }

        public override void OnOpen()
        {
            UIUtil.uiBattleRoot.DOScale(Vector3.one, 0.5f);
        }

        public override void OnClose()
        {

        }

        public override void OnDestroy()
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
            });
            inputListenser.onUp.AddListener((data) =>
            {
                InputManager.Instance.Push(new InputData(InputType.Up, UIUtil.FormatToVirtual(data)));
            });
            inputListenser.onDrag.AddListener((data) =>
            {
                InputManager.Instance.Push(new InputData(InputType.Drag, UIUtil.FormatToVirtual(data)));
            });
        }

        private void OnGameProcedure(EventGameProcedure procedure)
        {
            if (procedure.action == EventGameProcedure.ActionType.FinalWave)
            {
                ToastFinalWave();
            }
            else if (procedure.action == EventGameProcedure.ActionType.GameEndWin)
            {
                ShowGameEndPanel(true);
            }
            else if (procedure.action == EventGameProcedure.ActionType.GameEndLose)
            {
                ShowGameEndPanel(false);
            }
        }

        private void ToastFinalWave()
        {
            // TODO:TOAST
        }

        private void ShowGameEndPanel(bool isWin)
        {
            // TODO: game end
        }
    }
}