using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

// Output the build size or a failure depending on BuildPlayer.

public class WindowsBuilder : MonoBehaviour
{
    private static BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

    [MenuItem("Build/WindowsBuilder")]
    private static void Build()
    {
        // Place all your scenes here
        string[] scenes = new string[EditorBuildSettings.scenes.Length];

        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }

        buildPlayerOptions.scenes = scenes;

        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.None;
        buildPlayerOptions.locationPathName = "builds/Windows";


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
}
