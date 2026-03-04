#if EASY_UICONSOLE
using GAG.EasyUIConsole;
using UnityEngine;

namespace GAG.EasyTangibleTable.EasyUIConsole
{
    public class EasyTangibleTableEasyUIConsoleBridge : MonoBehaviour
    {
        void OnEnable()
        {
            EasyTangibleTableLogger.LogMsg += OnLog;
            EasyTangibleTagEvents.OpenConsoleRequested += OpenConsole;
        }
        
        void OnDisable()
        {
            EasyTangibleTableLogger.LogMsg -= OnLog;
            EasyTangibleTagEvents.OpenConsoleRequested -= OpenConsole;
        }
        
        void OnLog(string msg, EasyTangibleTableLogType type)
        {
            switch (type)
            {
                case EasyTangibleTableLogType.Highlight: EasyUIC.Highlight(msg); break;
                case EasyTangibleTableLogType.Warning: EasyUIC.Warning(msg); break;
                case EasyTangibleTableLogType.Error: EasyUIC.Error(msg); break;
                default: EasyUIC.Log(msg); break;
            }
        }
        
         void OpenConsole()
        {
            EasyUIC.OpenConsole();
        }
    }
}
#endif