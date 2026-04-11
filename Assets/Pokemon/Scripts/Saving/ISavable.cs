using UnityEngine;

namespace Pokemon.Scripts.Saving
{
    public interface ISavable
    {
        void CaptureState();
        bool RestoreState();
    }
}