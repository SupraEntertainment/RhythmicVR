namespace BlockSong {
	
	[System.Serializable]
	public class Obstacle {
		public float time = 1;
		public float xPos = 1;
		public float yPos = 0;
		public float height = 1;
		public float width = 0;
		public float length = 0;
		public int type = 0;

		public Obstacle(float time, float xPos, float yPos, float height, float width, float length, int type) {
			this.time = time;
			this.xPos = xPos;
			this.yPos = yPos;
			this.height = height;
			this.width = width;
			this.length = length;
			this.type = type;
		}
	}
}
