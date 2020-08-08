using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Sweet_And_Salty_Studios
{
    public enum SoundType
    {
        Missing,
        BackgroundMusic,
        PositiveSFX,
        MenuOpen
    }

    [Serializable]
    public abstract class Sound
    {
        public SoundType SoundType;
        public AudioClip AudioClip;
        [Range(0f, 1f)] public float Volume = 0.5f;
        [Range(-3f, 3f)] public float Pitch = 1f;
        [Range(-1f, 1f)] public float StereoPan = 0f;

        protected AudioSource audioSource;

        public virtual void Initialize(AudioMixerGroup audioMixerGroup)
        {
            audioSource = new GameObject(SoundType.ToString()).AddComponent<AudioSource>();

            audioSource.clip = AudioClip;
            audioSource.outputAudioMixerGroup = audioMixerGroup;

            audioSource.playOnAwake = false;
            audioSource.volume = Volume;
            audioSource.pitch = Pitch;
        }

        public abstract void Play(Vector2 position);
    }

    [Serializable]
    public class SoundEffect : Sound
    {
        [Range(0f, 1f)] public float SpatialBlend = 0f;
        [Range(0f, 1.1f)] public float ReverbZoneMix = 1f;

        [Space]
        [Header("3D Sound Settings")]
        [Range(0f, 5f)] public float DopplerEffect = 1f;
        [Range(0f, 360f)] public float Spread = 0f;

        public override void Initialize(AudioMixerGroup audioMixerGroup)
        {
            base.Initialize(audioMixerGroup);

            audioSource.spatialBlend = SpatialBlend;
            audioSource.reverbZoneMix = ReverbZoneMix;

            audioSource.dopplerLevel = DopplerEffect;
            audioSource.spread = Spread;
        }

        public override void Play(Vector2 position)
        {
            audioSource.transform.position = position;

            if(audioSource.isPlaying)
            {
                return;
            }

            audioSource.Play();
        }
    }

    [Serializable]
    public class MusicTrack : Sound
    {     
        public bool IsLooping;

        public override void Initialize(AudioMixerGroup audioMixerGroup)
        {
            base.Initialize(audioMixerGroup);

            audioSource.loop = IsLooping;
        }

        public override void Play(Vector2 position)
        {
            audioSource.transform.position = position;

            if(audioSource.isPlaying)
            {
                return;
            }

            audioSource.Play();
        }
    }

    public class AudioManager : Singelton<AudioManager>
    {
        #region VARIABLES

        public MusicTrack[] MusicTracks;
        public SoundEffect[] SoundEffects;
        public SoundEffect[] UI_SoundEffects;
        public SoundEffect MissingSound;

        private AudioMixer audioMixer;
        private AudioMixerGroup[] audioMixerGroups;

        private Hashtable gameSounds = new Hashtable();

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            audioMixer = Resources.Load<AudioMixer>("Audio/AudioMixer/AudioMixer");
            audioMixerGroups = audioMixer.FindMatchingGroups(string.Empty);
        }

        private void Start()
        {
            InitializeAudioMixerGroups();

            InitializeGameSounds();
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private void InitializeGameSounds()
        {
            MissingSound.Initialize(audioMixerGroups[0]);

            var index = 0;

            for(index = 0; index < MusicTracks.Length; index++)
            {
                MusicTracks[index].Initialize(audioMixerGroups[1]);
                gameSounds.Add(MusicTracks[index].SoundType, MusicTracks[index]);
            }

            for(index = 0; index < SoundEffects.Length; index++)
            {
                SoundEffects[index].Initialize(audioMixerGroups[2]);
                gameSounds.Add(SoundEffects[index].SoundType, SoundEffects[index]);
            }

            for(index = 0; index < UI_SoundEffects.Length; index++)
            {
                UI_SoundEffects[index].Initialize(audioMixerGroups[2]);
                gameSounds.Add(UI_SoundEffects[index].SoundType, UI_SoundEffects[index]);
            }
        }

        private void InitializeAudioMixerGroups()
        {
            foreach(var item in audioMixerGroups)
            {
                GetSavedAudioGroupValue(item);
            }
        }

        public void PlaySound(SoundType soundType, Vector2 position = new Vector2())
        {
            if(gameSounds.Contains(soundType))
            {
                var sound = gameSounds[soundType] as Sound;
                sound.Play(position);
            }
            else
            {
                MissingSound.Play(position);
                Debug.LogError($"{soundType} is missing!");
            }
        }

        private void GetSavedAudioGroupValue(AudioMixerGroup audioMixerGroup)
        {
            var name = audioMixerGroup.name;
            var value = PlayerPrefs.GetFloat(name);
            audioMixer.SetFloat(name, value);
        }

        public void ChangeAudioMixerGroupVolume(AudioMixerGroup audioMixerGroup)
        {
            var name = audioMixerGroup.name;
            audioMixer.GetFloat(name, out float value);
            audioMixer.SetFloat(name, value = value == 0 ? -80 : 0);
            PlayerPrefs.SetFloat(name, value);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}
