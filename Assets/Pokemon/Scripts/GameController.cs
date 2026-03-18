using UnityEngine;

namespace Pokemon.Scripts
{
    public enum GameState
    {
        Map,
        Battle,
    }
    public class GameController : MonoBehaviour
    {

        public Camera loungeCamera;
        public Camera battleCamera;
        private void Start()
        {

        }
    }
}