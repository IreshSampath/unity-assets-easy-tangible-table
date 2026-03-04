using UnityEngine;

namespace GAG.EasyTangibleTable
{
    public static class EasyTT
    {
        public static void OpenConsole()
        {
#if EASY_UICONSOLE
            EasyTangibleTagEvents.RaiseOpenConsoleRequested();
#else
        Debug.Log("Requires EasyUIConsole");
#endif
        }
    }
}