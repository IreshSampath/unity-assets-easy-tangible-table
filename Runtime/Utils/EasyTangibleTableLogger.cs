using System;
using UnityEngine;

namespace GAG.EasyTangibleTable
{
    public enum EasyTangibleTableLogType
    {
        Log,
        Highlight,
        Warning,
        Error
    }

    public static class EasyTangibleTableLogger
    {
        public static event Action<string, EasyTangibleTableLogType> LogMsg;

        public static void Print(string msg, EasyTangibleTableLogType type = EasyTangibleTableLogType.Log)
        {
#if UNITY_EDITOR
           // Debug.Log(msg);
#endif
            LogMsg?.Invoke(msg, type);
        }

        public static void Log(string msg) => Print(msg, EasyTangibleTableLogType.Log);
        public static void Highlight(string msg) => Print(msg, EasyTangibleTableLogType.Highlight);
        public static void Warning(string msg) => Print(msg, EasyTangibleTableLogType.Warning);
        public static void Error(string msg) => Print(msg, EasyTangibleTableLogType.Error);
    }
}
