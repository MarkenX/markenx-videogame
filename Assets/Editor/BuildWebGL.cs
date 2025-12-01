using UnityEditor;
using UnityEngine;

public class BuildWebGL
{
	public static void PerformBuild()
	{
		string[] scenes = { "Assets/Scenes/MainMenu.unity" };

		string buildPath = "/root/build/WebGL";

		BuildPlayerOptions options = new BuildPlayerOptions
		{
			scenes = scenes,
			locationPathName = buildPath,
			target = BuildTarget.WebGL,
			options = BuildOptions.Development  // Agrega BuildOptions.Development para debug
		};

		BuildPipeline.BuildPlayer(options);
		Debug.Log("WebGL build completado en: " + buildPath);
	}
}