using UnityEngine;

namespace Pokemon.Scripts.MyUtils.ObjectPooling
{
    public class ReturnToPool : MonoBehaviour
    {
        [HideInInspector] public MyPool pool;
        public void OnDisable()
        {
            pool.AddToPool(gameObject);
        }
    }
}