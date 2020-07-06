using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RhythmicVR {
	/// <summary>
	/// Defines a settings UI element
	/// </summary>
	[Serializable]
	public class SettingsField {
		public string name;
		public UiType type;
		public GameObject prefab;
		public GameObject initializedObject;
		public int maxValue = 1;
		public int minValue;
		[NonSerialized] public UnityAction buttonCall;
		[NonSerialized] public UnityAction<float> floatCall;
		[NonSerialized] public UnityAction<string> stringCall;
		[NonSerialized] public UnityAction<int, float> vectorNCall; // int = enumerator, float = value
		[NonSerialized] public UnityAction<int> enumCall;
		public string menuPath;

		public SettingsField(string name = null, UiType type = default, GameObject prefab = null, string menuPath = null) {
			this.name = name;
			this.type = type;
			this.prefab = prefab;
			this.menuPath = menuPath;
		}

		public void SetupListeners() {
			InputField input;
			Slider slider;
			
			switch (type) {
				case UiType.Button:
					initializedObject.GetComponentInChildren<Button>().onClick.AddListener(buttonCall);
					break;
				case UiType.Vector3:
					var allFloatInputs = initializedObject.GetComponentsInChildren<InputField>();
					for (var i = 0; i < allFloatInputs.Length; i++) {
						var inputElement = allFloatInputs[i];
						int i2 = i;
						inputElement.onValueChanged.AddListener(delegate(string arg0) { vectorNCall(i2, float.Parse(arg0)); });
					}
					break;
				case UiType.Color:
					break;
				case UiType.Text:
					initializedObject.GetComponentInChildren<InputField>().onValueChanged.AddListener(stringCall);
					break;
				case UiType.Int:
				case UiType.Float:
					input = initializedObject.GetComponentInChildren<InputField>();
					slider = initializedObject.GetComponentInChildren<Slider>();
					
					input.onValueChanged.AddListener(delegate(string arg0) { 
						floatCall(float.Parse(arg0));
						slider.value = float.Parse(arg0);
					});
					slider.onValueChanged.AddListener(delegate(float arg0) { 
						floatCall(arg0);
						input.text = arg0.ToString();
					});
					break;
				case UiType.Enum:
					initializedObject.GetComponentInChildren<Dropdown>().onValueChanged.AddListener(enumCall);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}