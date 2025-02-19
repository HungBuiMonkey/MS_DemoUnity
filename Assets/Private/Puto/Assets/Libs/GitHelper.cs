using System.IO;
using UnityEditor;
using UnityEngine;

public static class GitHelper
{
    public static string GetRepositoryHash()
    {
        #if UNITY_EDITOR
        return PlayerSettings.iOS.buildNumber;
        #else
        var gitPath = Path.Combine(Application.streamingAssetsPath, "build.txt");
        var gitStr = File.ReadAllText(gitPath);
        return gitStr;
        #endif
    }
}