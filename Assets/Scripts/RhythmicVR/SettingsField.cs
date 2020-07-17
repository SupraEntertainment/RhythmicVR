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
		public float maxValue = 1;
		public float minValue;
		[NonSerialized] public UnityAction buttonCall;
		[NonSerialized] public UnityAction<float> floatCall;
		[NonSerialized] public UnityAction<string> stringCall;
		[NonSerialized] public UnityAction<int, float> vectorNCall; // int = enumerator, float = value
		[NonSerialized] public UnityAction<int> enumCall;
		[NonSerialized] public UnityAction<bool> boolCall;
		public string menuPath;
		private object initialValue;

		/// <summary>
		/// Initialize the ui field
		/// </summary>
		/// <param name="name">String used as label</param>
		/// <param name="type">The type of element to use (int, string, bool, etc)</param>
		/// <param name="prefab">The prefab to use for generating that element</param>
		/// <param name="menuPath">The path for in which menu to place this element</param>
		/// <param name="initialValue">The initial value (e.g. for int: 30, for string: "C:/myFolder", for Dropdown/Enum: 2(integer index))</param>
		public SettingsField(string name = null, UiType type = default, GameObject prefab = null, string menuPath = null,object initialValue = null) {
			this.name = name;
			this.type = type;
			this.prefab = prefab;
			this.menuPath = menuPath;
			this.initialValue = initialValue;
		}

		public void SetupListeners() {
			InputField input;
			Slider slider;
			
			switch (type) {
				case UiType.Button:
					initializedObject.GetComponentInChildren<Button>().onClick.AddListener(buttonCall);
					break;
				case UiType.Vector3:
					float[] initialValues = new []{0f};
					if (initialValue != null) {
						var initialValuesVec3 = (Vector3) initialValue;
						 initialValues = new[] { initialValuesVec3.x, initialValuesVec3.y, initialValuesVec3.z };
					}
					var allFloatInputs = initializedObject.GetComponentsInChildren<InputField>();
					for (var i = 0; i < allFloatInputs.Length; i++) {
						var inputElement = allFloatInputs[i];
						int i2 = i;
						if (initialValue != null) inputElement.text = initialValues[i2].ToString();
						inputElement.onValueChanged.AddListener(delegate(string arg0) { vectorNCall(i2, float.Parse(arg0)); });
					}
					break;
				case UiType.Color:
					break;
				case UiType.Text:
					var textInput = initializedObject.GetComponentInChildren<InputField>();
					if (initialValue != null) textInput.text = (string) initialValue;
					textInput.onValueChanged.AddListener(stringCall);
					break;
				case UiType.Int:
					input = initializedObject.GetComponentInChildren<InputField>();
					slider = initializedObject.GetComponentInChildren<Slider>();
					
					if (initialValue != null) {
						input.text = ((int) initialValue).ToString();
						slider.value = (int) initialValue;
					}
					
					input.onValueChanged.AddListener(delegate(string arg0) { 
						floatCall(float.Parse(arg0));
						slider.value = float.Parse(arg0);
					});
					slider.onValueChanged.AddListener(delegate(float arg0) { 
						floatCall(arg0);
						input.text = arg0.ToString();
					});
					break;
				case UiType.Float:
					input = initializedObject.GetComponentInChildren<InputField>();
					slider = initializedObject.GetComponentInChildren<Slider>();
					
					if (initialValue != null) {
						input.text = ((float) initialValue).ToString();
						slider.value = (float) initialValue;
					}
					
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
					var dropdown = initializedObject.GetComponentInChildren<Dropdown>();
					
					if (initialValue != null) {
						dropdown.value = (int) initialValue;
					}
					
					dropdown.onValueChanged.AddListener(enumCall);
					break;
				case UiType.Bool:
					var toggle = initializedObject.GetComponentInChildren<Toggle>();
					
					if (initialValue != null) {
						toggle.isOn = (bool) initialValue;
					}

					toggle.onValueChanged.AddListener(boolCall);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}