using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildWebGL
{
	public static void PerformBuild()
	{
		string[] scenes = FindEnabledScenes();  // Todas las escenas enabled
		string buildPath = "Builds/WebGL";  // Path de salida (ajusta si usas /root/build)

		BuildPlayerOptions options = new BuildPlayerOptions
		{
			scenes = scenes,
			locationPathName = buildPath,
			target = BuildTarget.WebGL,
			options = BuildOptions.None  // O BuildOptions.Development para debug
		};

		var report = BuildPipeline.BuildPlayer(options);
		if (report.summary.result == BuildResult.Succeeded)
		{
			Debug.Log("WebGL build succeeded: " + buildPath);
		}
		else
		{
			Debug.LogError("Build failed: " + report.summary.result);
		}
	}

	private static string[] FindEnabledScenes()
	{
		var scenes = new System.Collections.Generic.List<string>();
		foreach (var scene in EditorBuildSettings.scenes)
		{
			if (scene.enabled) scenes.Add(scene.path);
		}
		return scenes.ToArray();
	}
}