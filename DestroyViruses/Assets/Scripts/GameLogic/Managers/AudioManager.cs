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
        //开火音乐
        public AudioSource FireMusicPlayer;
        //音效播放器
        public AudioSource SoundPlayer;

        private string mLastMusic = "";
        //播放音乐
        public void PlayMusic(string name, float volume = 1f, bool loop = true)
        {
            if (mLastMusic != name || !MusicPlayer.isPlaying)
            {
                AudioClip clip = Resources.Load<AudioClip>("Sounds/" + name);
                MusicPlayer.clip = clip;
                MusicPlayer.loop = loop;
                MusicPlayer.volume = volume;
                MusicPlayer.Play();
                mLastMusic = name;
            }
        }

        public void StopMusic()
        {
            if (MusicPlayer.isPlaying)
                MusicPlayer.Stop();
        }

        private string mLastFireMusic = "";
        //播放音乐
        public void PlayFireMusic(string name, float volume = 1f, bool loop = true)
        {
            if (mLastFireMusic != name || !FireMusicPlayer.isPlaying)
            {
                AudioClip clip = Resources.Load<AudioClip>("Sounds/" + name);
                FireMusicPlayer.clip = clip;
                FireMusicPlayer.loop = loop;
                FireMusicPlayer.volume = volume;
                FireMusicPlayer.Play();
                mLastFireMusic = name;
            }
        }

        public void StopFireMusic()
        {
            if (FireMusicPlayer.isPlaying)
                FireMusicPlayer.Stop();
        }

        //播放音效
        public void PlaySound(string name)
        {
            AudioClip clip = Resources.Load<AudioClip>("Sounds/" + name);
            SoundPlayer.clip = clip;
            SoundPlayer.PlayOneShot(clip);
        }

        public void StopSound()
        {
            SoundPlayer.Stop();
        }
    }
}