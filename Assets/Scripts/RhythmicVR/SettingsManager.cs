using System.Collections.Generic;
using UnityEngine;

namespace RhythmicVR {
	public class SettingsManager {
		public List<SettingsField> settings;
		public GameObject settingsMenuParent;
		private GameManager gm;

		public SettingsManager(GameManager gm) {
			this.gm = gm;
		}
		
		public void UpdateSettingsUi() {
			GameObject.Instantiate(gm.uiManager.scrollList, settingsMenuParent.transform.GetChild(0).position, new Quaternion());
		}
	}
}