using System;
using System.Collections.Generic;
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
            List<PkmType> strongTypes = TypeChart.GetStrongType(type);
            List<PkmType> weakTypes = TypeChart.GetWeakType(type);
            for (int i = 0; i < strongIcon.Length; i++)
            {
                if (i < strongTypes.Count)
                {
                    strongIcon[i].sprite = typeData.symbols.FirstOrDefault(x => x.type == strongTypes[i]).icon;
                    strongIcon[i].gameObject.SetActive(true);
                }
                else
                {
                    strongIcon[i].gameObject.SetActive(false);
                }
            }
            for (int i = 0; i < weakIcon.Length; i++)
            {
                if (i < weakTypes.Count)
                {
                    weakIcon[i].sprite = typeData.symbols.FirstOrDefault(x => x.type == weakTypes[i]).icon;
                    weakIcon[i].gameObject.SetActive(true);
                }
                else
                {
                    weakIcon[i].gameObject.SetActive(false);
                }
            }
        }

    }
}