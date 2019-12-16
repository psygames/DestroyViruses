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

        public Button baseOptionBtn;
        public Button aircraftOptionBtn;
        public Button coinOptionBtn;

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
            mTotalDrag = Vector2.zero;
            UIUtil.aircraftTransform.DOScale(Vector3.one * 1.3f, 0.5f).SetEase(Ease.OutQuad);
            RefreshUI();
            AudioManager.Instance.PlayMusic($"Sounds/BGM{Random.Range(1, 3)}", 1f);
            NavigationView.BlackSetting(false);
        }

        private void OnDestroy()
        {
            inputListenser.onDrag.RemoveAllListeners();
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
        }

        private void ButtonListenerInit()
        {
            baseOptionBtn.OnClick(() => { SelectOption(0); });
            aircraftOptionBtn.OnClick(() => { SelectOption(1); });
            coinOptionBtn.OnClick(() => { SelectOption(2); });
        }

        private void SelectOption(int optionIndex)
        {
            if (optionIndex == 0 || optionIndex == 1)
            {
                UIManager.Instance.Open<UpgradeView>();
            }
        }

        private void RefreshUI()
        {
            gameLevelPanel.SetData();
        }

        private void OnEventGameData(EventGameData evt)
        {
            if (evt.action == EventGameData.Action.DataChange)
            {
                RefreshUI();
            }
        }
    }
}