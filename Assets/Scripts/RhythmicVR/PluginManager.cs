using System.Collections.Generic;
using UnityEngine;

namespace RhythmicVR {
	public class PluginManager {
		private readonly List<AssetPackage> loadedPlugins = new List<AssetPackage>();
		
		private readonly List<Gamemode> loadedGamemodes = new List<Gamemode>();
		private readonly List<GameObject> loadedEnvironments = new List<GameObject>();
		private readonly List<GenericTrackedObject> loadedTrackedObjects = new List<GenericTrackedObject>();
		private readonly List<TargetObject> loadedTargetObjects = new List<TargetObject>();
		//private readonly List<MiscPlugin> miscPlugins;

		/// <summary>
		/// Add a plugin to the list
		/// </summary>
		/// <param name="plugin">The plugin to add</param>
		public void AddPlugin(AssetPackage plugin) {
			loadedPlugins.Add(plugin);
			switch (plugin.type) {
				case AssetType.Environment:
					loadedEnvironments.Add(plugin.gameObject);
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
		
		/// <summary>
		/// Return all gamemodes
		/// </summary>
		/// <returns></returns>
		public List<Gamemode> GetAllGamemodes() {
			return loadedGamemodes;
		}
		/// <summary>
		/// Return all Environment Prefabs
		/// </summary>
		/// <returns></returns>
		public List<GameObject> GetAllEnvironments() {
			return loadedEnvironments;
		}
		/*public List<AssetPackage> GetAllMiscelaneousPlugins() {
			return miscPlugins;
		}*/
		public List<GenericTrackedObject> GetAllTrackedObjects() {
			return loadedTrackedObjects;
		}
		public List<TargetObject> GetAllTargets() {
			return loadedTargetObjects;
		}
	}
}