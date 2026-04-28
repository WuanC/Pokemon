using Pokemon.Scripts.Pokemon;
using Pokemon.Scripts.FReward;

namespace Pokemon.Scripts.Character
{
    public class NPCBattle : NPCBase
    {
        public Party party;
        public Reward reward;
        public override void Awake()
        {
            base.Awake();
            party = GetComponent<Party>();
            party.Initialize();
        }
    }
}