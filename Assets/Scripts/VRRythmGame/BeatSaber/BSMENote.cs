namespace VRRythmGame.BeatSaber {
	[System.Serializable]
	public class BSMENote {
		private float _time;
		private int _lineIndex;
		private float _lineLayer;
		private int _type;
		private float _cutDirection;

		public Note ToNote() {
			return new Note(_time, 
			                _lineIndex/2 -2, 
			                _lineLayer/2f, 
			                _type == 0 ? new []{TrackingPoint.LeftHand} : new TrackingPoint[]{TrackingPoint.RightHand}, 
			                _cutDirection);
		}
	}
}
