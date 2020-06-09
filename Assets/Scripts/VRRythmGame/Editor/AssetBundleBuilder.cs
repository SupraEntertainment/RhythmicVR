using UnityEditor;
using UnityEngine;

namespace VRRythmGame.Editor {
    
    [ExecuteInEditMode]
    public class AssetBundleBuilder : MonoBehaviour {
        
        public string assetBundleOutPath;
        
        public void BuildGamemode() {
            var path = assetBundleOutPath + "/Gamemodes";
            if (!EnsureDirectoryIntegrity(path)) {
                return;
            }
            BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        }
        
        public void BuildTargetObject() {
            var path = assetBundleOutPath + "/TargetObjects";
            if (!EnsureDirectoryIntegrity(path)) {
                return;
            }
            BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        }
        
        public void BuildObstacle() {
            var path = assetBundleOutPath + "/Obstacles";
            if (!EnsureDirectoryIntegrity(path)) {
                return;
            }
            BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        }
        
        public void BuildTrackedObject() {
            var path = assetBundleOutPath + "/TrackedObjects";
            if (!EnsureDirectoryIntegrity(path)) {
                return;
            }
            BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        }

        public bool EnsureDirectoryIntegrity(string path) {
            if (System.IO.Directory.Exists(path)) {
                return true;
            }
            return false;
        }
        
    }
}
