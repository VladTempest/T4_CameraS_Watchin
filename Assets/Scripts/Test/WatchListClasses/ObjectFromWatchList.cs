using UnityEngine;

namespace Test.WatchListClasses
{
    public class ObjectFromWatchList : MonoBehaviour
    {
        private void Start()
        {
            WatchList.watchList.Add(transform);
        }
    }
}
