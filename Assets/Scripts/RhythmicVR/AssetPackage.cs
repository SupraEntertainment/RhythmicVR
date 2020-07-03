using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RhythmicVR {
	/// <summary>
	/// Parent object in any asset package to determine type and name
	/// </summary>
	public class AssetPackage : MonoBehaviour {

		public string packageName;
		public AssetType type;
		public Object unityAssetObject;
		[SerializeField] public object assetObject;

		private void Start() {
			if (unityAssetObject != null) {
				assetObject = unityAssetObject;
			}
		}
	}
}