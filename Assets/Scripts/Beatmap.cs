[System.Serializable]
public class Beatmap {
	public string formatVersion;
	public float version;
	public float[][] speed;
	public MapEvent[] events;
	public Note[] notes;
	public Obstacle[] obstacles;

	public Beatmap(string formatVersion = "1.0", float version = 1, float[][] speed = null, MapEvent[] events = null, Note[] notes = null, Obstacle[] obstacles = null) {
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

	public Beatmap(MapEvent[] events, Note[] notes, Obstacle[] obstacles) {
		this.events = events;
		this.notes = notes;
		this.obstacles = obstacles;
	}
}
