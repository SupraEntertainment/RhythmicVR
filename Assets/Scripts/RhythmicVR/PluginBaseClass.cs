using UnityEngine;

namespace RhythmicVR {
    public class PluginBaseClass : MonoBehaviour {

        public string pluginName;
        public AssetType type;
        public Object unityAssetObject;
        protected Core core;
        public object assetObject;

        private void Start() {
            if (unityAssetObject != null) {
                assetObject = unityAssetObject;
            }
        }
    
        /// <summary>
        /// Always call the base method. Applies Core object to property
        /// </summary>
        /// <param name="core">Game Core object</param>
        public virtual void Init(Core core) {
            this.core = core;
        }
    }
}
