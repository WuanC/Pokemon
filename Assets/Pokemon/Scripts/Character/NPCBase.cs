using UnityEngine;

namespace Pokemon.Scripts.Character
{
    public class NPCBase : MonoBehaviour
    {
        public NPCData npcData;

        protected SpriteRenderer npcAvatar;
        public virtual void Awake()
        {
            npcAvatar = GetComponent<SpriteRenderer>();

        }
        public void SetupNPCData()
        {
            npcAvatar.sprite = npcData.npcAvatar;
        }
#if UNITY_EDITOR
        void OnValidate()
        {
            if (npcData != null)
            {
                npcAvatar = GetComponent<SpriteRenderer>();
                npcAvatar.sprite = npcData.npcAvatar;
            }
        }
#endif
    }
}