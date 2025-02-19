using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class MyCustomBuildProcessor :IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
    
    public void OnPreprocessBuild(BuildReport report)
    {
        var currentBuildNumber = Convert.ToInt64(PlayerSettings.iOS.buildNumber);
        currentBuildNumber += 1;
        Debug.Log($"Set build number to {currentBuildNumber}");
        PlayerSettings.iOS.buildNumber = currentBuildNumber.ToString();
        PlayerSettings.Android.bundleVersionCode = Convert.ToInt32(currentBuildNumber);
        
        Debug.Log("Preprocessing Build...");
        var gitPath = Path.Combine(Application.streamingAssetsPath, "build.txt");
        var str = PlayerSettings.iOS.buildNumber;
        File.WriteAllText(gitPath, str);
    }
}