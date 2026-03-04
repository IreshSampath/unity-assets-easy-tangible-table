using UnityEngine;

namespace GAG.EasyTangibleTable
{
    public class AppBootstrap : MonoBehaviour
    {
        void Awake()
        {
            Application.runInBackground = true;
            Input.simulateMouseWithTouches = false;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}
