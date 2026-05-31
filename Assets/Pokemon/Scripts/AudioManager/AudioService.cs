using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.Scripts.AudioManager
{
    public class AudioService : MonoBehaviour
    {
        private const string SFX_KEY = "SFX_VOLUME";
        private const string MUSIC_KEY = "MUSIC_VOLUME";

        public static AudioService Instance;

        [SerializeField] private AudioDatabase database;

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        private Dictionary<AudioId, AudioData> soundDict;
        private AudioId currentMusicId;
        private Coroutine musicRoutine;
        private int musicVolume = 1;
        private int sfxVolume = 1;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            soundDict = new Dictionary<AudioId, AudioData>();

            foreach (var sound in database.sounds)
            {
                soundDict[sound.id] = sound;
            }
        }
        private void Start()
        {
            musicVolume = PlayerPrefs.GetInt(MUSIC_KEY, 1);
            sfxVolume = PlayerPrefs.GetInt(SFX_KEY, 1);
            PlayMusic(AudioId.BGM_Game);
        }
        public void PlaySFX(AudioId id)
        {
            if (!soundDict.TryGetValue(id, out var sound))
                return;
            sfxSource.PlayOneShot(
                sound.clip,
                sound.volume * sfxVolume
            );
        }
        public void PlaySfx(AudioData sound)
        {
            sfxSource.PlayOneShot(
                sound.clip,
                sound.volume * sfxVolume
            );
        }
        public void PlayMusic(AudioId id)
        {
            if (!soundDict.TryGetValue(id, out var sound))
                return;

            currentMusicId = id;
            musicSource.clip = sound.clip;
            musicSource.volume = sound.volume * musicVolume;
            musicSource.loop = true;

            musicSource.Play();
        }
        public void PlayMusic(AudioId id, System.Action onComplete)
        {
            if (!soundDict.TryGetValue(id, out var sound))
                return;
            currentMusicId = id;
            musicSource.clip = sound.clip;
            musicSource.loop = false;
            musicSource.Play();

            if (musicRoutine != null)
                StopCoroutine(musicRoutine);

            if (onComplete != null)
                musicRoutine = StartCoroutine(TrackMusic(onComplete));
        }

        private IEnumerator TrackMusic(System.Action callback)
        {
            yield return new WaitWhile(() => musicSource.isPlaying);

            callback?.Invoke();
        }
        private void UpdateMusicVolume()
        {
            if (!soundDict.TryGetValue(currentMusicId, out var sound))
                return;

            musicSource.volume = sound.volume * musicVolume;
        }
        public int GetMusicVolume()
        {
            int volume = PlayerPrefs.GetInt(MUSIC_KEY, 1);
            return volume;
        }
        public int GetSFXVolume()
        {
            int volume = PlayerPrefs.GetInt(SFX_KEY, 1);
            return volume;
        }
        public void ToggleMusic()
        {
            musicVolume = musicVolume == 1 ? 0 : 1;
            PlayerPrefs.SetInt(MUSIC_KEY, musicVolume);
            UpdateMusicVolume();
        }
        public void ToggleSFX()
        {
            sfxVolume = sfxVolume == 1 ? 0 : 1;
            PlayerPrefs.SetInt(SFX_KEY, sfxVolume);
        }
    }
}