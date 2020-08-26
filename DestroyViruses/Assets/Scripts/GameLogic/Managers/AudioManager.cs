using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class AudioManager : Singleton<AudioManager>
    {
        Dictionary<string, AudioClip> mCached = new Dictionary<string, AudioClip>();

        //音乐播放器
        [SerializeField]
        private AudioSource _MusicPlayer = null;
        //开火音乐
        [SerializeField]
        private AudioSource _FireMusicPlayer = null;
        //音效播放器
        [SerializeField]
        private AudioSource _SoundPlayer = null;

        private string mLastMusic = "";
        //播放音乐
        private void _PlayMusic(string name, float volume = 1f, bool loop = true)
        {
            if (mLastMusic != name || !_MusicPlayer.isPlaying)
            {
                AudioClip clip = GetClip(name);
                _MusicPlayer.clip = clip;
                _MusicPlayer.loop = loop;
                _MusicPlayer.volume = volume;
                _MusicPlayer.Play();
                mLastMusic = name;
            }
        }

        private void _StopMusic()
        {
            if (_MusicPlayer.isPlaying)
                _MusicPlayer.Stop();
        }

        private string mLastFireMusic = "";
        //播放音乐
        private void _PlayFireMusic(string name, float volume = 1f, bool loop = true)
        {
            if (mLastFireMusic != name || !_FireMusicPlayer.isPlaying)
            {
                AudioClip clip = GetClip(name);
                _FireMusicPlayer.clip = clip;
                _FireMusicPlayer.loop = loop;
                _FireMusicPlayer.volume = volume;
                _FireMusicPlayer.Play();
                mLastFireMusic = name;
            }
        }

        private void _StopFireMusic()
        {
            if (_FireMusicPlayer.isPlaying)
                _FireMusicPlayer.Stop();
        }

        //播放音效
        private void _PlaySound(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            AudioClip clip = GetClip(name);
            _SoundPlayer.clip = clip;
            _SoundPlayer.PlayOneShot(clip);
        }

        private void _StopSound()
        {
            _SoundPlayer.Stop();
        }

        private AudioClip GetClip(string clipName)
        {
            if (!mCached.TryGetValue(clipName, out AudioClip clip))
            {
                clip = ResourceUtil.Load<AudioClip>(PathUtil.Sound(clipName));
                mCached.Add(clipName, clip);
            }
            return clip;
        }

        public static AudioSource MusicPlayer => Instance._MusicPlayer;
        public static AudioSource FireMusicPlayer => Instance._FireMusicPlayer;
        public static AudioSource SoundPlayer => Instance._SoundPlayer;

        public static void PlayMusic(string name, float volume = 1f, bool loop = true)
        {
            Instance._PlayMusic(name, volume, loop);
        }

        public static void StopMusic()
        {
            Instance._StopMusic();
        }

        //播放音乐
        public static void PlayFireMusic(string name, float volume = 1f, bool loop = true)
        {
            Instance._PlayFireMusic(name, volume, loop);
        }

        public static void StopFireMusic()
        {
            Instance._StopFireMusic();
        }

        //播放音效
        public static void PlaySound(string name)
        {
            Instance._PlaySound(name);
        }

        public static void StopSound()
        {
            Instance._StopSound();
        }

        public static void Preload(string name)
        {
            Instance.GetClip(name);
        }
    }
}