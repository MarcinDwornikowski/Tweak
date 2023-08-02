using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tweak
{
    /// <summary>
    /// Tweak owner. Eg. for inactive objects.
    /// </summary>
    public class TweakMono : MonoBehaviour
    {
        [HideInInspector]
        public bool KeepOnLoad;

        private void Start()
        {
            if (KeepOnLoad)
                DontDestroyOnLoad(this.gameObject);
        }
    }

    /// <summary>
    /// TweakMono is for creating Tweak instance on a temp, always active gameobject.
    /// Global is a single instance meant to be used by multiple scripts.
    /// </summary>
    public static class GenericTweakOwner
    {
        private static int _instances = 0;
        private static Tweak _globalTweak;

        public static TweakMono Create()
        {
            if (!Application.isPlaying)
                return null;

            GameObject obj = new GameObject();
            obj.name = "temp: tweak mono " + _instances.ToString();
            TweakMono tweakMono = obj.AddComponent<TweakMono>();
            _instances++;
            return tweakMono;
        }

        public static Tweak Global()
        {
            if (!Application.isPlaying)
                return null;

            if (_globalTweak == null)
            {
                GameObject obj = new GameObject();
                obj.name = "temp: tweak mono global";
                
                TweakMono tweakMono = obj.AddComponent<TweakMono>();
                tweakMono.KeepOnLoad = true;
                _globalTweak = new Tweak(tweakMono, 10);
            }

            return _globalTweak;
        }
    }
}