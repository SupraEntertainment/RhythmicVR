using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RhythmicVR {
	public class PluginManager {
		private readonly List<PluginBaseClass> loadedPlugins = new List<PluginBaseClass>();
		
		private readonly List<Gamemode> loadedGamemodes = new List<Gamemode>();
		private readonly List<GameObject> loadedEnvironments = new List<GameObject>();
		private readonly List<GenericTrackedObject> loadedTrackedObjects = new List<GenericTrackedObject>();
		private readonly List<TargetObject> loadedTargetObjects = new List<TargetObject>();
		private readonly List<PluginBaseClass> miscPlugins = new List<PluginBaseClass>();

		private readonly Core core;

		public PluginManager(Core core) {
			this.core = core;
		}

		/// <summary>
		/// Add a plugin to the list
		/// </summary>
		/// <param name="plugin">The plugin to add</param>
		public void AddPlugin(PluginBaseClass plugin) {
			try {
				loadedPlugins.Add(plugin);
				switch (plugin.type) {
					case AssetType.Environment:
						loadedEnvironments.Add(plugin.gameObject);
						break;
					case AssetType.Gamemode:
						loadedGamemodes.Add(plugin.GetComponentInChildren<Gamemode>());
						plugin.Init(core);
						break;
					case AssetType.Misc:
						miscPlugins.Add(plugin);
						plugin.Init(core);
						break;
					case AssetType.TargetObject:
						loadedTargetObjects.Add(plugin.GetComponentInChildren<TargetObject>());
						break;
					case AssetType.VisualTrackedObject:
						loadedTrackedObjects.Add(plugin.GetComponentInChildren<GenericTrackedObject>());
						break;
				}
			}
			catch (Exception e) {
				Debug.Log("could not load Plugin: " + plugin.pluginName);
				Debug.Log(e);
			}
		}

		public void AddPlugins(PluginBaseClass[] plugins) {
			foreach (var plugin in plugins) {
				AddPlugin(plugin);
			}
		}

		public PluginBaseClass Find(string searchString) {
			return loadedPlugins.FirstOrDefault(plugin => plugin.pluginName == searchString);
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