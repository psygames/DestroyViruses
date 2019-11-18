using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class OptionView : ViewBase
    {
        public RadioObjects musicRadio;
        public RadioObjects vibraitonRadio;
        public Button musicBtn;
        public Button vibrationBtn;
        public Dropdown languageDropdown;

        private List<TableLanguage> mLangs = null;
        private void Awake()
        {
            mLangs = TableLanguage.GetAll().ToList();
            musicBtn.OnClick(OnClickMusic);
            vibrationBtn.OnClick(OnClickVibration);
            languageDropdown.onValueChanged.AddListener(OnValueChangedLanguage);
        }

        private void OnClickMusic()
        {
            Option.music = !Option.music;
            Option.sound = Option.music;
            Refresh();
        }

        private void OnClickVibration()
        {
            Option.vibration = !Option.vibration;
            Refresh();
        }

        private void OnValueChangedLanguage(int index)
        {
            Option.language = mLangs[index].id;
            foreach (var lt in UIUtil.uiRoot.GetComponentsInChildren<LocalizationText>())
            {
                lt.Validate();
            }
            Refresh();
        }

        private void Refresh()
        {
            musicRadio.Radio(Option.music);
            vibraitonRadio.Radio(Option.vibration);
            languageDropdown.options.Clear();
            foreach (var t in mLangs)
            {
                languageDropdown.options.Add(new Dropdown.OptionData(t.name));
            }
            languageDropdown.value = mLangs.FindIndex(a => a.id == Option.language);
            languageDropdown.captionText.text = mLangs[languageDropdown.value].name;
            SetMusic();
        }

        private void SetMusic()
        {
            AudioManager.Instance.MusicPlayer.mute = !Option.music;
            AudioManager.Instance.FireMusicPlayer.mute = !Option.sound;
            AudioManager.Instance.SoundPlayer.mute = !Option.sound;
        }

        protected override void OnOpen()
        {
            Refresh();
        }
    }
}