using UnityEngine;
using UnityEngine.Events;

namespace RhythmicVR {
	/// <summary>
	/// Defines a settings UI element
	/// </summary>
	[System.Serializable]
	public class SettingsField {
		public string name;
		public UiType type;
		public GameObject prefab;
		public GameObject initializedObject;
		[System.NonSerialized] public UnityEvent call = new UnityEvent();
		[System.NonSerialized] public int _inputIndex;
		[System.NonSerialized] public string _input;
		public string menuPath;
		//public SettingsField[] children;

		public void InvokeEvent(int inputIndex, string input) {
			_inputIndex = inputIndex;
			_input = input;
			call.Invoke();
		}
	}
}