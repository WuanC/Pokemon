using System;
using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts.Quest.Objective
{
    [CreateAssetMenu(fileName = "TypeObjective", menuName = "Pokemon/Quest/Objective/TypeObjective")]
    public class TypeObjective : ObjectiveBase
    {
        public PkmType pkmType;
    }
}