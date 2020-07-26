using RhythmicVR.Files;
using UnityEditor;
using UnityEngine;

namespace RhythmicVR.Editor {
    
    [CustomEditor(typeof(PluginBaseClass))]
    public class AssetPackageInspector : UnityEditor.Editor {


        public override void OnInspectorGUI() {
            
            var assetPackage = (PluginBaseClass)serializedObject.targetObject;
            
            DrawDefaultInspector();
            
            if (GUILayout.Button("Build AssetBundle")) {
                
                var uniqueID = GUID.Generate();
                var prefab = PrefabUtility.SaveAsPrefabAsset(assetPackage.gameObject, $"Assets/Plugin_{uniqueID}.prefab", out bool succeeded);
                
                if (succeeded)
                {
                    var bundle = new Bundle();
                    bundle.Prefab = prefab;
                    bundle.Name = prefab.gameObject.name;

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
