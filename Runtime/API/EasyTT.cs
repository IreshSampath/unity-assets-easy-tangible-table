using System;
using System.Collections.Generic;
using UnityEngine;

namespace GAG.EasyTangibleTable
{
    public static class EasyTT
    {
        public static event Action<IReadOnlyList<int>> ActiveTagsUpdated
        {
            add => EasyTangibleTagEvents.ActiveTagsUpdated += value;
            remove => EasyTangibleTagEvents.ActiveTagsUpdated -= value;
        }

        public static event Action<int> TagAligned
        {
            add => EasyTangibleTagEvents.TagAligned += value;
            remove => EasyTangibleTagEvents.TagAligned -= value;
        }

        public static event Action<int> TagAlignmentLost
        {
            add => EasyTangibleTagEvents.TagAlignmentLost += value;
            remove => EasyTangibleTagEvents.TagAlignmentLost -= value;
        }

        public static event Action<int> TagRemoved
        {
            add => EasyTangibleTagEvents.TagRemoved += value;
            remove => EasyTangibleTagEvents.TagRemoved -= value;
        }

        public static event Action<EasyTangibleTagModel> TagUpdated
        {
            add => EasyTangibleTagEvents.TagUpdated += value;
            remove => EasyTangibleTagEvents.TagUpdated -= value;
        }

        public static event Action<int, Vector2> TagMoved
        {
            add => EasyTangibleTagEvents.TagMoved += value;
            remove => EasyTangibleTagEvents.TagMoved -= value;
        }

        public static event Action<int, float> TagRotated
        {
            add => EasyTangibleTagEvents.TagRotated += value;
            remove => EasyTangibleTagEvents.TagRotated -= value;
        }

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