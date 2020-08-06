using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildProject : ScriptableObject {
	public bool isDebug;
	public string windowsOutDir = "out_win";
	public string linuxOutDir = "out_lin";
	
	public BuildProject() {
		BuildOptions buildOptions = BuildOptions.None;

		if (isDebug) {
			windowsOutDir += "_debug";
			linuxOutDir += "_debug";
			
			//set build options
			buildOptions &= BuildOptions.ConnectWithProfiler & BuildOptions.Development;
		}
		
		//windows
		BuildPipeline.BuildPlayer(new []{SceneManager.GetSceneByName("inGame").path}, windowsOutDir, BuildTarget.StandaloneWindows64, buildOptions);
		
		//linux
		BuildPipeline.BuildPlayer(new []{SceneManager.GetSceneByName("inGame").path}, linuxOutDir, BuildTarget.StandaloneLinux64, buildOptions);
	}
}
