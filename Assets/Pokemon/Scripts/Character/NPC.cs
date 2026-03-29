using UnityEngine;

namespace Pokemon.Scripts.Character
{
    public class NPC : MonoBehaviour
    {
        public NPCData npcData;
        public Pokemon.Party party;
        [SerializeField] private SpriteRenderer npcAvatar;
        public void SetupNPCData()
        {
            npcAvatar.sprite = npcData.npcAvatar;
        }
    }
}