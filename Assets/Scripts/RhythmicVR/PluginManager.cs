using System.Collections.Generic;
using UnityEngine;

namespace RhythmicVR {
	public class PluginManager {
		private readonly List<PluginBaseClass> loadedPlugins = new List<PluginBaseClass>();
		
		private readonly List<Gamemode> loadedGamemodes = new List<Gamemode>();
		private readonly List<GameObject> loadedEnvironments = new List<GameObject>();
		private readonly List<GenericTrackedObject> loadedTrackedObjects = new List<GenericTrackedObject>();
		private readonly List<TargetObject> loadedTargetObjects = new List<TargetObject>();
		private readonly List<PluginBaseClass> miscPlugins = new List<PluginBaseClass>();

		private Core core;

		public PluginManager(Core core) {
			this.core = core;
		}

		/// <summary>
		/// Add a plugin to the list
		/// </summary>
		/// <param name="plugin">The plugin to add</param>
		public void AddPlugin(PluginBaseClass plugin) {
			loadedPlugins.Add(plugin);
			switch (plugin.type) {
				case AssetType.Environment:
					loadedEnvironments.Add(plugin.gameObject);
					break;
				case AssetType.Gamemode:
					loadedGamemodes.Add(plugin.GetComponentInChildren<Gamemode>());
					break;
				case AssetType.Misc:
					miscPlugins.Add(plugin);
					plugin.Init(core);
					break;
				case AssetType.TargetObject:
					loadedTargetObjects.Add(plugin.GetComponentInChildren<TargetObject>());
					break;
				case AssetType.TrackedObject:
					loadedTrackedObjects.Add(plugin.GetComponentInChildren<GenericTrackedObject>());
					break;
			}
		}

		public void AddPlugins(PluginBaseClass[] plugins) {
			foreach (var plugin in plugins) {
				AddPlugin(plugin);
			}
		}
		
		public List<PluginBaseClass> GetPlugins() {
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
		public List<PluginBaseClass> GetAllMiscelaneousPlugins() {
			return miscPlugins;
		}
		public List<GenericTrackedObject> GetAllTrackedObjects() {
			return loadedTrackedObjects;
		}
		public List<TargetObject> GetAllTargets() {
			return loadedTargetObjects;
		}
	}
}