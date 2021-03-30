using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;
using System;

// Output the build size or a failure depending on BuildPlayer.

public class Windows64Builder : MonoBehaviour
{
    private static BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

    
    private static void Build()
    {
        // Place all your scenes here
        string[] scenes = new string[EditorBuildSettings.scenes.Length];

        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }

        buildPlayerOptions.scenes = scenes;

        buildPlayerOptions.options = BuildOptions.None;

        EditorUserBuildSettings.development = false;
        EditorUserBuildSettings.allowDebugging = false;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
    [MenuItem("Build/Windows/64bit")]
    private static void Windows64()
    {
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.locationPathName = "builds/Windows_64bit/" + GetTime();

        Build();
    }
    [MenuItem("Build/Windows/32bit")]
    private static void Windows32()
    {
        buildPlayerOptions.target = BuildTarget.StandaloneWindows;
        buildPlayerOptions.locationPathName = "builds/Windows_32bit/" + GetTime();

        Build();
    }
    [MenuItem("Build/WebGL")]
    private static void WebGL()
    {
        buildPlayerOptions.target = BuildTarget.WebGL;
        buildPlayerOptions.locationPathName = "builds/WebGL/" + GetTime();

        Build();
    }

    private static string GetTime()
    {
        string curTime = DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss");
        return curTime;
    }
}
