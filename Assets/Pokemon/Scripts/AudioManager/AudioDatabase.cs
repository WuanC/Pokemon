using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.Scripts.AudioManager
{
    [CreateAssetMenu(menuName = "Audio/Audio Database")]
    public class AudioDatabase : ScriptableObject
    {
        public List<AudioData> sounds;
    }
}