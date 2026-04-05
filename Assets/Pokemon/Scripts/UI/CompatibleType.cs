using System;
using System.Linq;
using Pokemon.Scripts.Data;
using Pokemon.Scripts.Pokemon;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI
{
    public class CompatibleType : MonoBehaviour
    {
        [SerializeField] private TypeData typeData;
        [SerializeField] private Image[] strongIcon;
        [SerializeField] private Image[] weakIcon;
        [SerializeField] private Image currentIcon;

        public void SetupCompatibleType(PkmType type)
        {
            currentIcon.sprite = typeData.symbols.FirstOrDefault(x => x.type == type).icon;
        }

    }
}