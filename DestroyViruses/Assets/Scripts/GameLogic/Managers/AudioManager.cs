using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class AudioManager : Singleton<AudioManager>
    {
        //音乐播放器
        public AudioSource MusicPlayer;
        //音效播放器
        public AudioSource SoundPlayer;

        private string mLastMusic = "";
        //播放音乐
        public void PlayMusic(string name,float volume = 1f, bool loop = true)
        {
            if (mLastMusic != name)
            {
                AudioClip clip = Resources.Load<AudioClip>(name);
                MusicPlayer.clip = clip;
                MusicPlayer.loop = loop;
                MusicPlayer.volume = volume;
                MusicPlayer.Play();
                mLastMusic = name;
            }
        }

        public void StopMusic()
        {
            MusicPlayer.Stop();
        }

        //播放音效
        public void PlaySound(string name)
        {
            AudioClip clip = Resources.Load<AudioClip>(name);
            SoundPlayer.clip = clip;
            SoundPlayer.PlayOneShot(clip);
        }

        public void StopSound()
        {
            SoundPlayer.Stop();
        }
    }
}