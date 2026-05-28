using Pokemon.Scripts.Pokemon;
using Pokemon.Scripts.UI.Screens;
using UnityEngine;

namespace Pokemon.Scripts.Tutorial
{
    public class ChosePokemonScreen : ScreenBase
    {
        [SerializeField] private ChosePokemonPanel[] chosePokemonPanels;
        public void InitScreen(PokemonParty[] configs)
        {
            base.Active();
            for (int i = 0; i < chosePokemonPanels.Length; i++)
            {
                if (i < configs.Length)
                {
                    chosePokemonPanels[i].gameObject.SetActive(true);
                    chosePokemonPanels[i].InitPanel(configs[i], this);
                }
                else
                {
                    chosePokemonPanels[i].gameObject.SetActive(false);
                }
            }
        }
        public void DisableScreen()
        {
            base.Deactive();
            TutorialManager.Instance.CompleteChosePokemon();
        }
    }
}