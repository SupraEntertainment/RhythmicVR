/*using System;
using UnityEditor;
using UnityEngine;

namespace VRRythmGame.Editor {
    
    [CustomEditor(typeof(AssetPackage))]
    public class AssetPackageInspector : UnityEditor.Editor {
        
        
        private SerializedProperty packageName;
        private SerializedProperty type;
        
        private void OnEnable() {
            
            packageName = serializedObject.FindProperty("packageName");
            type = serializedObject.FindProperty("type");
        }


        public override void OnInspectorGUI() {
            
            var assetPackage = (AssetPackage)serializedObject.targetObject;
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(packageName);
            EditorGUILayout.PropertyField(type);
            EditorGUILayout.PropertyField(unityAssetObject);
            EditorGUILayout.EndVertical();
            if (GUILayout.Button("Build AssetBundle")) {
                var abb = new AssetBundleBuild();
                
                
                
                var uniqueID = GUID.Generate();
                var prefab = PrefabUtility.SaveAsPrefabAsset(assetPackage.gameObject, $"Assets/Avatar_{uniqueID}.prefab", out bool succeeded);

                abb.assetNames = new string[] {prefab};
                AssetBundleBuild[] builds = new AssetBundleBuild[]{};
                BuildPipeline.BuildAssetBundles()
                
                if (succeeded)
                {
                    var bundle = new AssetBundle();
                    bundle.Avatar = prefab;
                    bundle.Name = avatar.gameObject.name;

                    AssetDatabase.CreateAsset(bundle, $"Assets/AvatarBundle_{uniqueID}.asset");
                    try
                    {
                        ModBuilder.BuildBundleFromAsset(bundle);
                    }
                    catch { }

                    AssetDatabase.DeleteAsset($"Assets/Avatar_{uniqueID}.prefab");
                    AssetDatabase.DeleteAsset($"Assets/AvatarBundle_{uniqueID}.asset");
                }
                else
                {
                    Debug.LogError("Failed to create avatar prefab");
                }
            }
        }
        
    }
}
*/