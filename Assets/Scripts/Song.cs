[System.Serializable]
public class Song {
	public string formatVersion;
	public float version;
	public float[][] speed;
	public Event[] events;
	public Note[] notes;
	public Obstacle[] obstacles;

	public Song(string formatVersion = "1.0", float version = 1, float[][] speed = null, Event[] events = null, Note[] notes = null, Obstacle[] obstacles = null) {
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

	public Song(Event[] events, Note[] notes, Obstacle[] obstacles) {
		this.events = events;
		this.notes = notes;
		this.obstacles = obstacles;
	}
}
