using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AirCraftCombat
{
    /// <summary>
    ///  All in game Utils method are declared here.
    /// </summary>
    public class Utils
    {
        public static void Log(object msg)
        {
#if UNITY_EDITOR
            Debug.Log(msg);
#endif
        }

        public static void LogWarning(object msg)
        {
#if UNITY_EDITOR
            Debug.LogWarning(msg);
#endif
        }

        public static void LogError(object msg)
        {
#if UNITY_EDITOR
            Debug.LogError(msg);
#endif
        }

    }
}
