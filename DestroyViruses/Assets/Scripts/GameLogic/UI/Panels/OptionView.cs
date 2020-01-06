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
                else if (_tag == LT.TAG_FR) val = t.fr;
                else if (_tag == LT.TAG_SP) val = t.sp;
                else if (_tag == LT.TAG_DE) val = t.de;
                else if (_tag == LT.TAG_RU) val = t.ru;
                else if (_tag == LT.TAG_JA) val = t.ja;
                else if (_tag == LT.TAG_KO) val = t.ko;
                else val = t.en;
                languageDropdown.options.Add(new Dropdown.OptionData(val));
            }
            languageDropdown.value = LT.Tags.IndexOf(Option.language);
            languageDropdown.captionText.text = LT.Tags[languageDropdown.value];
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