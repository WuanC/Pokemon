using System;
using System.Collections;
using System.Collections.Generic;
using Pokemon.Scripts.Character;
using Pokemon.Scripts.Data;
using Pokemon.Scripts.MyUtils;
using UnityEngine;

namespace Pokemon.Scripts.UI.Screens
{
    public class ScreenManager : Singleton<ScreenManager>
    {
        [SerializeField] private SplashScreen splashScreen;
        [SerializeField] private EnterHubScreen enterHubScreen;
        [SerializeField] private EnterBattleScreen enterBattleScreen;
        [SerializeField] private PlayScreen playScreen;
        [SerializeField] private EvolutionScreen evolutionScreen;

        public void EnterHubClick(Action goBtnAction, MapData mapData, MapConditionData mapCondition = null)
        {
            enterHubScreen.Initialize(goBtnAction, mapData, mapCondition);
        }
        public void EnterBattleClick(Action onFightBtnClick, NPC npc)
        {
            enterBattleScreen.Initialize(onFightBtnClick, npc);
        }
        public void ActiveSplashScreen(Action onComplete)
        {
            splashScreen.onFadeComplete = onComplete;
            splashScreen.Fade();
        }
        public IEnumerator Evolution(List<PairPokemonEvolution> pairEvolutions)
        {

            yield return evolutionScreen.Evolution(pairEvolutions);
        }
        public void EnterDetailMap()
        {
            playScreen.EnterDetailMap();
        }
    }
}