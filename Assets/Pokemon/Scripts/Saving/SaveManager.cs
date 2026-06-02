using System.Linq;
using UnityEngine;

namespace Pokemon.Scripts.Saving
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private ISavable[] savables;

        private void Awake()
        {
            savables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .OfType<ISavable>()
                .ToArray();
            Debug.Log($"Found {savables.Length} savable objects.");
        }
        private void OnApplicationFocus(bool focus)
        {
            if (!focus)
            {
                foreach (var savable in savables)
                {
                    savable.CaptureState();
                }
            }
        }
        private void OnApplicationQuit()
        {
            foreach (var savable in savables)
            {
                savable.CaptureState();
            }
        }
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                foreach (var savable in savables)
                {
                    savable.CaptureState();
                }
            }
        }
    }
}