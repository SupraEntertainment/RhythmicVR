using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Valve.Newtonsoft.Json.Utilities;

namespace RhythmicVR.Editor {
    
    public class AssetBundleBuilder {
        
        public static string assetBundleOutPath;

        public static void BuildAssetBundles(GameObject conentes) {
            AssetBundleBuild[] builds = new AssetBundleBuild[]{};
            var abb = new AssetBundleBuild();
            builds.AddDistinct(abb);
            
            var uniqueID = GUID.Generate();
            var prefab = PrefabUtility.SaveAsPrefabAsset(conentes, $"Assets/Asset_{uniqueID}.prefab", out bool succeeded);
            
            abb.assetNames = new string[] {prefab.name};
            abb.assetBundleName = conentes.GetComponent<AssetPackage>().packageName;
            
            // halp, how 2 mark the asset in that prefab for usage in the asset bundle?
            
            var path = assetBundleOutPath + "/";
            switch (prefab.GetComponent<AssetPackage>().type) {
                case AssetType.Gamemode:
                    path += "Gamemodes";
                    break;
                case AssetType.TargetObject:
                    path += "TargetObjects";
                    break;
                case AssetType.Environment:
                    path += "Environments";
                    break;
                case AssetType.TrackedObject:
                    path += "TrackedObjects";
                    break;
                case AssetType.Misc:
                    path += "Misc";
                    break;
                default:
                    path += "Misc";
                    break;
            }
            if (!EnsureDirectoryIntegrity(path)) {
                return;
            }
            BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
            
            AssetDatabase.DeleteAsset($"Assets/Asset_{uniqueID}.prefab");
        }

        public static bool EnsureDirectoryIntegrity(string path) {
            if (System.IO.Directory.Exists(path)) {
                return true;
            }
            return false;
        }
        
    }
}
