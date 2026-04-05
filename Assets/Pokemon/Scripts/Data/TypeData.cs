using System.Collections.Generic;
using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts.Data
{
    [CreateAssetMenu(fileName = "TypeData", menuName = "Pokemon/TypeData")]
    public class TypeData : ScriptableObject
    {
        public List<TypeItem> symbols;
    }
    [System.Serializable]
    public class TypeItem
    {
        public PkmType type;
        public Sprite icon;
    }
}