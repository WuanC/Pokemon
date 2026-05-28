using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Pokemon.Scripts.Tutorial
{
    [CreateAssetMenu(fileName = "TutorialConfig", menuName = "Pokemon/TutorialConfig", order = 0)]
    public class TutorialConfig : ScriptableObject
    {
        public TutorConfigType tutorConfigType;
        [ShowIf("tutorConfigType", TutorConfigType.Talk)]
        public List<TalkConfig> talkConfigs;
        [System.Serializable]
        public class TalkConfig
        {
            public string npcName;
            [TextArea(3, 10)]
            public string dialogue;
            public Sprite npcSprite;
        }

    }
    public enum TutorConfigType
    {
        Talk,
    }

}