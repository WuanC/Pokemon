using UnityEngine;

namespace Pokemon.Scripts.MyUtils
{
    public enum EventId
    {
        //On Start Battle
        OnEncounterPokemon,
        OnEncounterTrainer,
        OnEndBattle,

        //Switch Pokemon
        OnSwitchPokemon,

        //Item
        OnItemUsed,
        OnItemUsedInBattle,

        //Noti
        OnShowMessage,



    }
}