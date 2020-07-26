using RhythmicVR.Files;
using UnityEditor;
using UnityEngine;

namespace RhythmicVR.Editor {
	[CustomEditor(typeof(BuildPlugin))]
	public class BuildPluginInspector : UnityEditor.Editor  {

		public override void OnInspectorGUI() {
            
			var assetPackage = ((BuildPlugin)serializedObject.targetObject).pluginBaseClass;
            
			if (GUILayout.Button("Build AssetBundle")) {
                
				var uniqueID = GUID.Generate();
				var prefab = PrefabUtility.SaveAsPrefabAsset(assetPackage.gameObject, $"Assets/Plugin_{uniqueID}.prefab", out bool succeeded);
                
				if (succeeded)
				{
					var bundle = new Bundle();
					bundle.Prefab = prefab;
					bundle.Name = assetPackage.pluginName;

					AssetDatabase.CreateAsset(bundle, $"Assets/Plugin_{uniqueID}.asset");
					try
					{
						ModBuilder.BuildBundleFromAsset(bundle);
					}
					catch { }

					AssetDatabase.DeleteAsset($"Assets/Plugin_{uniqueID}.prefab");
					AssetDatabase.DeleteAsset($"Assets/Plugin_{uniqueID}.asset");
				}
				else
				{
					Debug.LogError("Failed to create avatar prefab");
				}
			}
		}
	}
}