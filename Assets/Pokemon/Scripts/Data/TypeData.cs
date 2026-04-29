using System.Collections.Generic;
using Pokemon.Scripts.Pokemon;
using UnityEngine;

namespace Pokemon.Scripts.Data
{
    [CreateAssetMenu(fileName = "TypeData", menuName = "Pokemon/TypeData")]
    public class TypeData : ScriptableObject
    {
        public List<TypeItem> symbols;
        public List<TypeItem> bigPanels;
        public List<TypeItem> thumbPanels;

        public Sprite GetBigPanel(PkmType type)
        {
            return bigPanels.Find(x => x.type == type)?.icon;
        }
        public Sprite GetThumbPanel(PkmType type)
        {
            return thumbPanels.Find(x => x.type == type)?.icon;
        }
    }
    [System.Serializable]
    public class TypeItem
    {
        public PkmType type;
        public Sprite icon;
    }
}