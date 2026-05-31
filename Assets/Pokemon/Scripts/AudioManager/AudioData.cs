using UnityEngine;

namespace Pokemon.Scripts.AudioManager
{
    [System.Serializable]
    public class AudioData
    {
        public AudioId id;
        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 1f;

        public bool loop;
    }
}