using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MineSweeper.Framework.game;
using MineSweeper.Framework.System;
using MineSweeper.Framework.UI;
using System;

namespace MineSweeper.Framework.System
{
    public enum ESound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public class SoundManager : Singleton<SoundManager>
    {
        #region 필드 변수

        AudioSource[] m_audioSources = new AudioSource[(int)ESound.MaxCount];
        Dictionary<string, AudioClip> m_audioClips = new Dictionary<string, AudioClip>();


        #endregion


        #region 유니티 고유 이벤트


        #endregion


        #region 함수

        // 초기화
        private void Initialize()
        {
            GameObject root = GameObject.Find("Audio");
            if (root == null)
            {
                root = new GameObject { name = "Audio" };
                DontDestroyOnLoad(root);

                string[] soundNames = Enum.GetNames(typeof(ESound)); // "Bgm", "Effect"

                for (int i = 0; i < soundNames.Length - 1; i++)
                {
                    GameObject go = new GameObject { name = soundNames[i] };
                    m_audioSources[i] = go.AddComponent<AudioSource>();
                    go.transform.parent = root.transform;
                }

                m_audioSources[(int)ESound.Bgm].loop = true; // bgm 재생기는 무한 반복 재생
            }
            root.transform.parent = transform;
        }

        // Clear
        public void Clear()
        {
            foreach (AudioSource audioSource in m_audioSources)
            {
                audioSource.clip = null;
                audioSource.Stop();
            }
            // 효과음 Dictionary 비우기
            m_audioClips.Clear();
        }

        // 재생
        public void Play(AudioClip audioClip, ESound type = ESound.Effect, float pitch = 1.0f)
        {
            if (audioClip == null)
                return;

            if (type == ESound.Bgm) // BGM 배경음악 재생
            {
                AudioSource audioSource = m_audioSources[(int)ESound.Bgm];
                if (audioSource.isPlaying)
                    audioSource.Stop();

                audioSource.pitch = pitch;
                audioSource.clip = audioClip;
                audioSource.Play();
            }
            else // Effect 효과음 재생
            {
                AudioSource audioSource = m_audioSources[(int)ESound.Effect];
                audioSource.pitch = pitch;
                audioSource.PlayOneShot(audioClip);
            }
        }

        public void Play(string path, ESound type = ESound.Effect, float pitch = 1.0f)
        {
            AudioClip audioClip = GetOrAddAudioClip(path, type);
            Play(audioClip, type, pitch);
        }

        private AudioClip GetOrAddAudioClip(string path, ESound type = ESound.Effect)
        {
            if (path.Contains("Sounds/") == false)
                path = $"Sounds/{path}";

            AudioClip audioClip = null;

            if (type == ESound.Bgm) // BGM 배경음악 클립 붙이기
            {
                audioClip = Resources.Load<AudioClip>(path);
            }
            else // Effect 효과음 클립 붙이기
            {
                if (m_audioClips.TryGetValue(path, out audioClip) == false)
                {
                    audioClip = Resources.Load<AudioClip>(path);
                    m_audioClips.Add(path, audioClip);
                }
            }

            if (audioClip == null)
                Debug.Log($"AudioClip Missing ! {path}");

            return audioClip;
        }



        #endregion


        #region 재정의 함수

        protected override void OnCreated()
        {
            base.OnCreated();
            Initialize();

            Play("BGM", ESound.Bgm);
        }

        #endregion


        #region 인터페이스 구현


        #endregion
    }
}

