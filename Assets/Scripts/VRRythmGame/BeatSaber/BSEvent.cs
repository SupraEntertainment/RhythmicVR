namespace VRRythmGame.BeatSaber {
	[System.Serializable]
	public class BSEvent {
		private float _time;
		private int _type;
		private int _value;

		public MapEvent ToEvent() {
			var mapEvent = new MapEvent();
			return mapEvent;
		}
	}
}
