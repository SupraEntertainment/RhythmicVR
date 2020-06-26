using UnityEngine;

namespace RhythmicVR {
	/// <summary>
	/// Parent object in any asset package to determine type and name
	/// </summary>
	public class AssetPackage : MonoBehaviour {

		public string packageName;
		public AssetType type;
	}
}