using UnityEngine;
using UnityEngine.UI;

namespace Pokemon.Scripts.UI
{
    public class StatGUI : MonoBehaviour
    {
        [SerializeField] private Image arrow;
        public void SetArrowDirection(bool isUp)
        {
            if (isUp)
            {
                arrow.transform.localScale = new Vector3(arrow.transform.localScale.x, -Mathf.Abs(arrow.transform.localScale.y), arrow.transform.localScale.z);
            }
            else
            {
                arrow.transform.localScale = new Vector3(arrow.transform.localScale.x, Mathf.Abs(arrow.transform.localScale.y), arrow.transform.localScale.z);
            }
        }
    }
}