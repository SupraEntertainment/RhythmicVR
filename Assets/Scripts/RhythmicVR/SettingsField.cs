namespace RhythmicVR {
	/// <summary>
	/// Defines a settings UI element
	/// </summary>
	[System.Serializable]
	public class SettingsField {
		public string name;
		public UiType type;
		public SettingsField[] children;
	}
}