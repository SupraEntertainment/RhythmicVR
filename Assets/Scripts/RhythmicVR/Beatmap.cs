namespace RhythmicVR {
	[System.Serializable]
	public class Beatmap {
		public string formatVersion;
		public float version;
		public float[][] speed; // how fast at which time -> 0;1 = speed 1 at time 0 ... 100;3 = speed 3 at time 100
		public string targetObject = "target_cube";
		public Note[] notes;

		public Beatmap(string formatVersion = "1.0", float version = 1, float[][] speed = null, Note[] notes = null) {
			Setup(formatVersion, version, speed, notes);
		}

		public Beatmap(Note[] notes) {
			Setup("1.0", 1, new float[][]{}, notes);
		}

		private void Setup(string formatVersion = "1.0", float version = 1, float[][] speed = null, Note[] notes = null) {
			this.formatVersion = formatVersion;
			this.version = version;
			if (speed == null) {
				speed = new []{ new float[]{0, 1} };
			}
			this.speed = speed;
			this.notes = notes;
		}
		
	}
}
