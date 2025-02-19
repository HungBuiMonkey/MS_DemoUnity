using System;
using UnityEngine;

public class LogMe
{

    public static bool isLogOn = true;

    static public void Log(object log)
    {
#if !FORCE_DEBUG_BUILD
        if (isLogOn)
#endif
            Debug.Log($"{log}");
    }
    public static void Log(Exception ex)
    {
#if !FORCE_DEBUG_BUILD
        if (isLogOn)
#endif
            Debug.LogException(ex);
    }
    static public void LogWarning(object log)
    {
#if !FORCE_DEBUG_BUILD
        if (isLogOn)
#endif
            Debug.LogWarning($"{log}");
    }

    static public void LogWarning(object log, UnityEngine.Object context)
    {
#if !FORCE_DEBUG_BUILD
        if (isLogOn)
#endif
            Debug.LogWarning($"{log}", context);
    }

    static public void LogError(object log)
    {
#if !FORCE_DEBUG_BUILD
        if (isLogOn)
#endif
            Debug.LogError($"{log}");
    }

    static public void LogError(object log, UnityEngine.Object context)
    {
#if !FORCE_DEBUG_BUILD
        if (isLogOn)
#endif
            Debug.LogError($"{log}", context);
    }

    static public void LogFormat(string format, params object[] args)
    {
#if !FORCE_DEBUG_BUILD
        if (isLogOn)
#endif
            Debug.LogFormat(format, args);
    }
}
