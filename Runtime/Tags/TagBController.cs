using UnityEngine;
using UnityEngine.UI;

namespace GAG.EasyTangibleTable
{
    public class TagBController : EasyTangibleTagControllerBase
    {
        [SerializeField] Button _playButton;
        [SerializeField] Button _infoButton;

        void Start()
        {
            _playButton.onClick.AddListener(OnPlay);
            _infoButton.onClick.AddListener(OnInfo);
        }

        void OnPlay() => Debug.Log("Play Video");
        void OnInfo() => Debug.Log("Show Info");
    }
}
