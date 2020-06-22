namespace RhythmicVR {
	[System.Serializable]
	public class Beatmap {
		public string formatVersion;
		public float version;
		public float[][] speed; // how fast at wich time -> 0;1 = speed 1 at time 0 ... 100;3 = speed 3 at time 100
		public Event[] events;
		public Note[] notes;
		public Obstacle[] obstacles;

		public Beatmap(string formatVersion = "1.0", float version = 1, float[][] speed = null, Event[] events = null, Note[] notes = null, Obstacle[] obstacles = null) {
			Setup(formatVersion, version, speed, events, notes, obstacles);
		}

		public Beatmap(Event[] events, Note[] notes, Obstacle[] obstacles) {
			Setup("1.0", 1, new float[][]{}, events, notes, obstacles);
		}

		private void Setup(string formatVersion = "1.0", float version = 1, float[][] speed = null, Event[] events = null, Note[] notes = null, Obstacle[] obstacles = null) {
			this.formatVersion = formatVersion;
			this.version = version;
			if (speed == null) {
				speed = new []{ new float[]{0, 1} };
			}
			this.speed = speed;
			this.events = events;
			this.notes = notes;
			this.obstacles = obstacles;
		}
		
	}
}
