namespace BeatSaber {
	[System.Serializable]
	public class BSNote {
		private float _time;
		private int _lineIndex;
		private float _lineLayer;
		private float _type;
		private float _cutDirection;

		public Note ToNote() {
			return new Note();
		}
	}
}
