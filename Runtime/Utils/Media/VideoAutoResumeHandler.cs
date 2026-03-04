using UnityEngine;
using UnityEngine.Video;

namespace GAG.EasyTangibleTable
{
    public class VideoAutoResumeHandler : MonoBehaviour
    {
        VideoPlayer _video;

        void Start()
        {
            _video = GetComponent<VideoPlayer>();
            _video.timeUpdateMode = VideoTimeUpdateMode.UnscaledGameTime;
            _video.skipOnDrop = true;
        }

        void Update()
        {
            if (!_video.isPlaying)
                _video.Play();
        }
    }
}