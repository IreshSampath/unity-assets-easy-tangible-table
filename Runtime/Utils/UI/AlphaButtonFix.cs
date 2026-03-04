using UnityEngine;
using UnityEngine.UI;

namespace GAG.EasyTangibleTable
{
    public class AlphaButtonFix : MonoBehaviour
    {
        void Start()
        {
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.2f;
        }
    }
}