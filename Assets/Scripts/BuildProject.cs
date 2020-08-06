using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildProject : EditorWindow {
	public bool isDebug = false;
	public string windowsOutDir = "out_win";
	public string linuxOutDir = "out_lin";
	
	public static void Build(bool isDebug, string windowsOutDir, string linuxOutDir) {
		BuildOptions buildOptions = BuildOptions.None;

		if (isDebug) {
			windowsOutDir += "_debug";
			linuxOutDir += "_debug";
			
			//set build options
			buildOptions &= BuildOptions.ConnectWithProfiler & BuildOptions.Development;
		}
		
		//windows
		BuildReport report = BuildPipeline.BuildPlayer(new []{SceneManager.GetSceneByName("Game").path}, windowsOutDir, BuildTarget.StandaloneWindows64, buildOptions);
		BuildSummary summary = report.summary;

		if (summary.result == BuildResult.Succeeded)
		{
			Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
		}

		if (summary.result == BuildResult.Failed)
		{
			Debug.Log("Build failed");
		}
		
		//linux
		report = BuildPipeline.BuildPlayer(new []{SceneManager.GetSceneByName("Game").path}, linuxOutDir, BuildTarget.StandaloneLinux64, buildOptions);
		summary = report.summary;

		if (summary.result == BuildResult.Succeeded)
		{
			Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
		}

		if (summary.result == BuildResult.Failed)
		{
			Debug.Log("Build failed");
		}
	}

	[MenuItem("Build/Build Project")]
	public static void ShowWindow() {
		GetWindow(typeof(BuildProject));
	}

	private void OnGUI() {
		isDebug = EditorGUILayout.Toggle("Is Debug Build", isDebug);
		windowsOutDir = EditorGUILayout.TextField("Windows Build Directory", windowsOutDir);
		linuxOutDir = EditorGUILayout.TextField("Linux Build Directory", linuxOutDir);

		if (GUILayout.Button("Build")) {
			Build(isDebug, windowsOutDir, linuxOutDir);
		}
	}
}
