using System;
using System.Collections.Generic;

namespace GAG.EasyTangibleTable
{
    public static class EasyTangibleTagEvents
    {
        public static event Action<EasyTangibleTagModel> TagPlaced;
        public static event Action<List<int>> TagAlived;
        public static event Action<int> TagRemoved;
        public static event Action OpenConsoleRequested;
        
        public static void RaiseTagPlaced(EasyTangibleTagModel easyTangibleTag) => TagPlaced?.Invoke(easyTangibleTag);

        public static void RaiseTagAlived(List<int> aliveTagIDs) => TagAlived?.Invoke(aliveTagIDs);
        
        public static void RaiseTagRemoved(int tagID) => TagRemoved?.Invoke(tagID);
        
        public static void RaiseOpenConsoleRequested() => OpenConsoleRequested?.Invoke();
    }
}
