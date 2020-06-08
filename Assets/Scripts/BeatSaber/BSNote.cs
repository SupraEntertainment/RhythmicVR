namespace BeatSaber {
	[System.Serializable]
	public class BSNote {
		private float _time;
		private int _lineIndex;
		private int _lineLayer;
		private int _type;
		private int _cutDirection;

		public Note ToNote() {
			return new Note(_time, 
			                _lineIndex/2 -2, 
			                _lineLayer/2f, 
			                _type == 0 ? new []{TrackingPoint.LeftHand} : new TrackingPoint[]{TrackingPoint.RightHand}, 
			                _cutDirection);
		}
	}
}
