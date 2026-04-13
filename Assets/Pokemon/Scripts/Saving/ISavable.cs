using UnityEngine;

namespace Pokemon.Scripts.Saving
{
    public interface ISavable
    {
        void CaptureState();
        object RestoreState();
    }
}