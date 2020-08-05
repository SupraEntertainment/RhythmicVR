using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RhythmicVR {
	public class PluginManager {
		private readonly List<PluginBaseClass> allPlugins = new List<PluginBaseClass>();
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
				bool existsAlready = false;
				for (var i = 0; i < allPlugins.Count; i++) {
					if (allPlugins[i].pluginName.Equals(plugin.pluginName)) {
						SetPluginActiveState(allPlugins[i], false);
						allPlugins[i] = plugin;
						existsAlready = true;
						break;
					}
				}

				if (!existsAlready) {
					allPlugins.Add(plugin);
				}
			}
			catch (Exception e) {
				Debug.Log("could not load Plugin: " + plugin.pluginName);
				Debug.Log(e);
			}
		}

		/// <summary>
		/// Enables or disables plugins properly
		/// </summary>
		/// <param name="plugin">the plugin to deal with</param>
		/// <param name="state">true = enable, false = disable</param>
		public void SetPluginActiveState(PluginBaseClass plugin, bool state) {
			if (state) { //enable

				// if dependencies are not met, don't load plugin
				if (!ResolveDependencies(plugin)) {
					return;
				}
				
				switch (plugin.type) {
					case AssetType.Environment:
						loadedEnvironments.Add(plugin.gameObject);
						break;
					case AssetType.Gamemode:
						var gamemode = core.SimpleInstantiate(plugin.gameObject).GetComponent<Gamemode>();
						loadedGamemodes.Add(gamemode);
						loadedPlugins.Add(gamemode);
						gamemode.Init(core);
						break;
					case AssetType.Misc:
						var pl = core.SimpleInstantiate(plugin.gameObject).GetComponent<PluginBaseClass>();
						miscPlugins.Add(pl);
						loadedPlugins.Add(pl);
						pl.Init(core);
						break;
					case AssetType.TargetObject:
						loadedTargetObjects.Add(plugin.GetComponentInChildren<TargetObject>());
						break;
					case AssetType.VisualTrackedObject:
						loadedTrackedObjects.Add(plugin.GetComponentInChildren<GenericTrackedObject>());
						break;
				}
				
			} else { //disable
				plugin.StopPlugin();
				switch (plugin.type) {
					case AssetType.Gamemode:
						break;
					case AssetType.TargetObject:
						break;
					case AssetType.Environment:
						break;
					case AssetType.VisualTrackedObject:
						break;
					case AssetType.Misc:
						Object.Destroy(plugin.gameObject);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="plugin">the plugin to check for dependencies</param>
		/// <returns>success</returns>
		private bool ResolveDependencies(PluginBaseClass plugin) {
			List<string> missingDependencies = new List<string>();
			if (plugin.dependencies != null) {
				foreach (var dependency in plugin.dependencies) {
					bool depMet = true;
					foreach (var plugin1 in allPlugins) {
						if (plugin1.pluginName.Equals(dependency)) {
							depMet = ResolveDependencies(plugin1);
							break;
						}
					}

					if (!depMet) {
						missingDependencies.Add(dependency);
					}
				}
			}

			if (missingDependencies.Count != 0) {
				Debug.Log("Failed to load " + plugin.assetName + " (" + plugin.pluginName + ") because of missing dependencies.\n" 
				        + plugin.assetName + " requires: " + missingDependencies);
				return false;
			}

			return true;
		}

		public void AddPlugins(PluginBaseClass[] plugins) {
			foreach (var plugin in plugins) {
				AddPlugin(plugin);
			}
			foreach (var plugin in plugins) {
				SetPluginActiveState(plugin, true);
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

		public void LoadPluginsFromFolder(string path) {
			List<PluginBaseClass> pluginsOut = new List<PluginBaseClass>();

			string tempPath = path + "temp/";

			Directory.CreateDirectory(tempPath);
			

			if (Util.EnsureDirectoryIntegrity(path, true)) {
				string[] pluginPaths = Directory.GetFiles(path);
				foreach (var pluginPath in pluginPaths) {
#if UNITY_EDITOR
					if (pluginPath.Contains(".meta")) {
						continue;
					}	
#endif
					string destPath = tempPath + Path.GetFileName(pluginPath);
					var zip = new FastZip();
					zip.ExtractZip(pluginPath, destPath, "");
					string destPathPlatform = destPath + "/";
#if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
					destPathPlatform += "win64";
#elif (UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX)
					destPathPlatform += "lin64";
#endif
					destPathPlatform = Directory.GetFiles(destPathPlatform)[0];
					AssetBundle assetBundle = AssetBundle.LoadFromFile(destPathPlatform);
					string[] assetNames = assetBundle.GetAllAssetNames();
					string assetName = "";
					foreach (var asset in assetNames) {
						if (asset.Contains("plugin") && asset.Contains(".prefab")) {
							assetName = asset;
							break;
						}
					}
					GameObject assetObject = assetBundle.LoadAsset<GameObject>(assetName);
					PluginBaseClass plugin = assetObject.GetComponentInChildren<PluginBaseClass>();
					pluginsOut.Add(plugin);
				}
			}

			try {
				Directory.Delete(tempPath);
			}
			catch (Exception e) {
				Debug.Log("Could not delete temporary plugin directory");
			}

			AddPlugins(pluginsOut.ToArray());
		}
	}
}