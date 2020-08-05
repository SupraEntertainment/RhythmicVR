using UnityEngine;

namespace RhythmicVR.Files
{
    public class Bundle : ScriptableObject
    {
        public GameObject Prefab;
        public string Name;
        public Object[] ExtraAssetIncludes;

        public BundleAssets AssetsInBundle;
    }

    [System.Serializable]
    public class BundleAssets
    {
        public Object[] AssetObjects;
        public string[] AssetPaths;
        public string[] ScenePaths;
    }

}
