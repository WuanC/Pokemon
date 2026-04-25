using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts.Character
{
    public class NPCBattle : NPCBase
    {
        public Party party;
        public override void Awake()
        {
            base.Awake();
            party = GetComponent<Party>();
            party.Initialize();
        }
    }
}