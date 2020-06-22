using System.Collections.Generic;

namespace VRRythmGame {
	public class PluginManager {
		private List<AssetPackage> loadedPlugins = new List<AssetPackage>();
		
		private List<Gamemode> loadedGamemodes = new List<Gamemode>();
		//private List<Environment> loadedPlugins;
		//private List<MiscPlugin> loadedPlugins;
		private List<GenericTrackedObject> loadedTrackedObjects = new List<GenericTrackedObject>();
		private List<TargetObject> loadedTargetObjects = new List<TargetObject>();

		public void AddPlugin(AssetPackage plugin) {
			loadedPlugins.Add(plugin);
			switch (plugin.type) {
				case AssetType.Environment:
					break;
				case AssetType.Gamemode:
					loadedGamemodes.Add(plugin.GetComponentInChildren<Gamemode>());
					break;
				case AssetType.Misc:
					break;
				case AssetType.TargetObject:
					loadedTargetObjects.Add(plugin.GetComponentInChildren<TargetObject>());
					break;
				case AssetType.TrackedObject:
					loadedTrackedObjects.Add(plugin.GetComponentInChildren<GenericTrackedObject>());
					break;
			}
		}
		
		public List<AssetPackage> GetPlugins() {
			return loadedPlugins;
		}
		
		public List<Gamemode> GetAllGamemodes() {
			return loadedGamemodes;
		}
		/*public List<AssetPackage> GetPlugins() {
			return loadedPlugins;
		}
		public List<AssetPackage> GetPlugins() {
			return loadedPlugins;
		}
		public List<AssetPackage> GetPlugins() {
			return loadedPlugins;
		}
		public List<AssetPackage> GetPlugins() {
			return loadedPlugins;
		}*/
	}
}