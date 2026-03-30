
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI.Screens
{
    public class WinScreen : MonoBehaviour
    {
        [SerializeField] private Image youWinImage;
        [SerializeField] private Image amazingImage;
        [SerializeField] private GameObject pokemonContainer;
        [SerializeField] private Image pokemonImage;
        [SerializeField] private Image detailPanelImage;
        [SerializeField] private TextMeshProUGUI pokemonNameText;
        [SerializeField] private Image xpBarImage;
    }
}