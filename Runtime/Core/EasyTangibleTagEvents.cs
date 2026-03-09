using System;
using System.Collections.Generic;
using UnityEngine;

namespace GAG.EasyTangibleTable
{
    public static class EasyTangibleTagEvents
    {
        public static event Action OpenConsoleRequested;
        
        public static event Action<EasyTangibleTagModel> TagPlaced;
        
        public static event Action<EasyTangibleTagModel> TagUpdated;
        public static event Action<int, Vector2> TagMoved;
        public static event Action<int, float> TagRotated;
        
        public static event Action<IReadOnlyList<int>> ActiveTagsUpdated;
        
        public static event Action<int> TagAligned;
        public static event Action<int> TagAlignmentLost;
        
        public static event Action<int> TagRemoved;
        
        public static void RaiseOpenConsoleRequested() => OpenConsoleRequested?.Invoke();
        
        public static void RaiseTagPlaced(EasyTangibleTagModel easyTangibleTag) => TagPlaced?.Invoke(easyTangibleTag);

        public static void RaiseTagUpdated(EasyTangibleTagModel tag) => TagUpdated?.Invoke(tag);
        
        public static void RaiseTagMoved(int tagID, Vector2 distance) => TagMoved?.Invoke(tagID, distance);

        public static void RaiseTagRotated(int tagID, float rotation) => TagRotated?.Invoke(tagID, rotation);
        
        public static void RaiseActiveTagsUpdated(IReadOnlyList<int> aliveTagIDs) => ActiveTagsUpdated?.Invoke(aliveTagIDs);
        
        public static void RaiseTagAligned(int tagID) => TagAligned?.Invoke(tagID);
        
        public static void RaiseTagAlignmentLost(int tagID) => TagAlignmentLost?.Invoke(tagID);
        
        public static void RaiseTagRemoved(int tagID) => TagRemoved?.Invoke(tagID);
        
    }
}
