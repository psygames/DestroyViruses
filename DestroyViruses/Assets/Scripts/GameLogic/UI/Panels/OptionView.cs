using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        public Text version;

        private void Awake()
        {
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
            Option.language = LT.Tags[index];
            Refresh();
        }

        private void Refresh()
        {
            musicRadio.Radio(Option.music);
            vibraitonRadio.Radio(Option.vibration);
            languageDropdown.options.Clear();
            var t = TableLanguage.Get("LANGUAGE_NAME");
            string val;
            foreach (var _tag in LT.Tags)
            {
                if (_tag == LT.TAG_EN) val = t.en;
                else if (_tag == LT.TAG_CN) val = t.cn;
                else val = t.en;
                languageDropdown.options.Add(new Dropdown.OptionData(val));
            }
            languageDropdown.value = LT.Tags.IndexOf(Option.language);
            SetMusic();
            version.text = $"{LTKey.VERSION.LT()}: {Application.version}";
        }

        private void SetMusic()
        {
            AudioManager.MusicPlayer.mute = !Option.music;
            AudioManager.FireMusicPlayer.mute = !Option.sound;
            AudioManager.SoundPlayer.mute = !Option.sound;
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            Refresh();
        }

        protected override void OnClose()
        {
            base.OnClose();
            AudioManager.PlaySound("button_normal");
        }
    }
}