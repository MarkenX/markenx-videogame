#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.IO;

public static class BuildWebGL
{
	public static void PerformBuild()
	{
		Debug.Log("=== Starting WebGL Build ===");

		// Verificar módulo WebGL
		if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.WebGL, BuildTarget.WebGL))
		{
			Debug.LogError("WebGL build target is not supported in this Unity installation!");
			EditorApplication.Exit(1);
		}

		string buildPath = "Build/WebGL";
		if (!Directory.Exists(buildPath))
			Directory.CreateDirectory(buildPath);

		string[] scenes = new string[] { "Assets/Scenes/MainMenu.unity" };

		// Verificar que las escenas existen
		foreach (var scenePath in scenes)
		{
			if (!File.Exists(scenePath))
			{
				Debug.LogError($"Scene not found: {scenePath}");
				EditorApplication.Exit(1);
			}
		}

		Debug.Log("Building scenes: " + string.Join(", ", scenes));

		BuildPlayerOptions options = new BuildPlayerOptions
		{
			scenes = scenes,
			locationPathName = buildPath,
			target = BuildTarget.WebGL,
			options = BuildOptions.None
		};

		BuildReport report = BuildPipeline.BuildPlayer(options);
		BuildSummary summary = report.summary;

		Debug.Log($"Build completed. Result: {summary.result}, Total size: {summary.totalSize} bytes");

		if (summary.result != BuildResult.Succeeded)
		{
			Debug.LogError("Build failed!");
			EditorApplication.Exit(1);
		}

		Debug.Log("=== WebGL Build Completed Successfully ===");
		EditorApplication.Exit(0);
	}
}
#endif
