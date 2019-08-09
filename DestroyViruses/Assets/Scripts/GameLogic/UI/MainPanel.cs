using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

namespace DestroyViruses
{
    public class MainPanel : UIPanel
    {
        public UIEventListener inputListenser;
        public float dragBeginThreshold = 100;

        public Button baseOptionBtn;
        public GameObject baseOptionBtnSelect;
        public Button aircraftOptionBtn;
        public GameObject aircraftOptionBtnSelect;
        public Button coinOptionBtn;
        public GameObject coinOptionBtnSelect;

        public GameObject basePanel;
        public Button firePowerUpBtn;
        public Text firePowerLevelText;
        public Text firePowerUpCostText;
        public Button fireSpeedUpBtn;
        public Text fireSpeedLevelText;
        public Text fireSpeedUpCostText;

        public GameObject aircraftPanel;
        public GameObject coinPanel;

        private Vector2 mTotalDrag = Vector2.zero;

        private void Awake()
        {
            InputListenerInit();
            ButtonListenerInit();
        }

        protected override void OnOpen()
        {
            mTotalDrag = Vector2.zero;
            UIUtil.uiBattleRoot.DOScale(Vector3.one * 1.2f, 0.5f);
        }

        protected override void OnClose()
        {
        }

        private void OnDestroy()
        {
            inputListenser.onDrag.RemoveAllListeners();
            inputListenser.onClick.RemoveAllListeners();
        }

        private void InputListenerInit()
        {
            inputListenser.onDrag.AddListener((data) =>
            {
                mTotalDrag += data;
                if (mTotalDrag.magnitude > dragBeginThreshold)
                {
                    GameManager.ChangeState<BattleState>();
                }
            });

            inputListenser.onClick.AddListener(_ =>
            {
                HideAllPanel();
            });
        }

        private void ButtonListenerInit()
        {
            baseOptionBtn.OnClick(() => { ShowPanel(basePanel, baseOptionBtnSelect); });
            aircraftOptionBtn.OnClick(() => { ShowPanel(aircraftPanel, aircraftOptionBtnSelect); });
            coinOptionBtn.OnClick(() => { ShowPanel(coinPanel, coinOptionBtnSelect); });

            firePowerUpBtn.OnClick(() =>
            {
                if (GameLocalData.Instance.coin < FormulaUtil.FirePowerUpCost(GameLocalData.Instance.firePowerLevel))
                {
                    return;
                }

                Debug.Log("level up");
            });

            fireSpeedUpBtn.OnClick(() =>
            {

            });
        }

        private void HideAllPanel()
        {
            basePanel.SetActive(false);
            baseOptionBtnSelect.SetActive(false);

            aircraftPanel.SetActive(false);
            aircraftOptionBtnSelect.SetActive(false);

            coinPanel.SetActive(false);
            coinOptionBtnSelect.SetActive(false);
        }

        public void ShowPanel(GameObject panel, GameObject btnSelect)
        {
            HideAllPanel();
            panel.SetActive(true);
            btnSelect.SetActive(true);
        }
    }
}