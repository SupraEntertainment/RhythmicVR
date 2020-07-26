using UnityEngine;

namespace RhythmicVR {
	
	[RequireComponent(typeof(PluginBaseClass))]
	public class BuildPlugin : MonoBehaviour {

		public PluginBaseClass pluginBaseClass;

		private void Start() {
			pluginBaseClass = GetComponent<PluginBaseClass>();
		}
	}
}