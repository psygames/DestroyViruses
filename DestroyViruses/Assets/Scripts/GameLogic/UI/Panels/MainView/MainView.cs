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
        // drag listener
        public UIEventListener inputListenser;

        // panels
        public GameLevelPanel gameLevelPanel;
        public LevelPanel levelPanel;
        public RadioObjects[] optionBtnRaido;
        public RadioObjects optionPanelRaido;

        public Button baseOptionBtn;
        public Button aircraftOptionBtn;
        public Button coinOptionBtn;

        // top menu
        public Text coinText;
        public Text energyText;
        public Text diamondText;
        public Button settingBtn;
        public Button debugBtn;

        // private
        private Vector2 mTotalDrag = Vector2.zero;
        private float mDragBeginThreshold = 100;


        private void Awake()
        {
            InputListenerInit();
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
            SelectOption(-1);
            mTotalDrag = Vector2.zero;
            UIUtil.aircraftTransform.DOScale(Vector3.one * 1.3f, 0.5f).SetEase(Ease.OutQuad);
            RefreshUI();
            AudioManager.Instance.PlayMusic($"Sounds/BGM{Random.Range(1, 3)}", 1f);
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
                if (mTotalDrag.magnitude > mDragBeginThreshold)
                {
                    StateManager.ChangeState<BattleState>();
                }
            });

            inputListenser.onClick.AddListener(_ =>
            {
                SelectOption(-1);
            });
        }

        private void ButtonListenerInit()
        {
            baseOptionBtn.OnClick(() => { SelectOption(0); });
            aircraftOptionBtn.OnClick(() => { SelectOption(1); });
            coinOptionBtn.OnClick(() => { SelectOption(2); });
            settingBtn.OnClick(() => { UIManager.Instance.Open<OptionView>(UILayer.Top); });
            debugBtn.OnClick(() => { UIManager.Instance.Open<DebugView>(UILayer.Top); });
        }

        private void SelectOption(int optionIndex)
        {
            for (int i = 0; i < optionBtnRaido.Length; i++)
            {
                optionBtnRaido[i].Radio(i == optionIndex);
            }
            optionPanelRaido.Radio(optionIndex);
        }

        private void RefreshUI()
        {
            coinText.text = GDM.ins.coin.KMB();
            energyText.text = "100";
            diamondText.text = "0";
            levelPanel.SetData();
            gameLevelPanel.SetData();
        }

        private void OnEventGameData(EventGameData evt)
        {
            if (evt.action == EventGameData.Action.DataChange)
            {
                RefreshUI();
            }
            else if (evt.action == EventGameData.Action.Error)
            {
                //TODO: TOAST ERROR
                Debug.Log(evt.errorMsg);
            }
        }
    }
}