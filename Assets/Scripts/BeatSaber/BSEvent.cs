namespace BeatSaber {
	[System.Serializable]
	public class BSEvent {
		private int _time;
		private int _type;
		private int _value;

		public Event ToEvent() {
			return new Event();
		}
	}
}
