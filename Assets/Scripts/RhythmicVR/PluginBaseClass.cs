using UnityEngine;

namespace RhythmicVR {
    [ExecuteInEditMode]
    public class PluginBaseClass : MonoBehaviour {

        public string pluginName;
        public string assetName;
        public AssetType type;
        
        protected Core core;

        private void Start() {
            if (!gameObject.GetComponent<BuildPlugin>()) {
                var buildPluginButton = gameObject.AddComponent<BuildPlugin>();
                buildPluginButton.pluginBaseClass = this;
            }
        }

        /*public Object unityAssetObject;
        public object assetObject;

        private void Start() {
            if (unityAssetObject != null) {
                assetObject = unityAssetObject;
            }
        }*/
    
        /// <summary>
        /// Always call the base method. Applies Core object to property
        /// </summary>
        /// <param name="core">Game Core object</param>
        public virtual void Init(Core core) {
            this.core = core;
        }
    }
}
