namespace VRRythmGame.BeatSaber {
	[System.Serializable]
	public class BSEvent {
		private int _time;
		private int _type;
		private int _value;

		public MapEvent ToEvent() {
			return new MapEvent();
		}
	}
}
