using System;
using UnityEngine;

namespace AudioManagerScript
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;
        public static AudioManager Instance
        {
            get
            {
                return instance;
            }
        }

        public void Awake()
        {
            if(instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public Sound[] sounds;
        private float masterVolumeMusic;
        private float masterVolumeSound;

        void Start()
        {
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.picth;
                s.source.loop = s.loop;
            }
        }

        public float GetMasterVolumeMusic()
        {
            return masterVolumeMusic;
        }

        public float GetMasterVolumeSound()
        {
            return masterVolumeSound;
        }

        public void SetMasterVolMusic(float value)
        {
            masterVolumeMusic = value;

            UpdateMasterVolumeMusic();
        }

        public void SetMasterVolSound(float value)
        {
            masterVolumeSound = value;

            UpdateMasterVolumeSounds();
        }

        private void UpdateMasterVolumeMusic()
        {
            foreach (Sound s in sounds)
            {
                if(s.isMusic)
                {
                    s.source.volume = masterVolumeMusic;
                }
            }
        }

        private void UpdateMasterVolumeSounds()
        {
            foreach (Sound s in sounds)
            {
                if (!s.isMusic)
                {
                    s.source.volume = masterVolumeSound;
                }
            }
        }

        public void Play(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (s != null)
            {
                if (!s.source.isPlaying)
                    s.source.Play();
            }
        }
        public void Stop(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (s != null)
            {
                if (s.source.isPlaying)
                    s.source.Stop();
            }
        }

        public void Pause(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (s != null)
            {
                if (s.source.isPlaying)
                    s.source.Pause();
            }
        }

        public void Resume(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (s != null)
            {
                if (!s.source.isPlaying)
                    s.source.UnPause();
            }
        }

        public void StopAllSFX()
        {
            foreach (Sound s in sounds)
            {
                Stop(s.name);
            }
        }
    }
}