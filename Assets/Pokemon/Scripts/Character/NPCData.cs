using System.Collections.Generic;
using JetBrains.Annotations;
using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts.Character
{
    [System.Serializable]
    public class NPCData
    {
        public string npcName;
        public Sprite npcSprite;
        public Sprite npcAvatar;
        public string npcMessage;
    }
}