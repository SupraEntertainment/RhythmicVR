namespace VRRythmGame {
    [System.Serializable]
    public class Note {
        public float time = 1;
        public float xPos = 1;
        public float yPos = 0;
        public float speed = 1;
        public TrackingPoint[] type = new TrackingPoint[]{};
        public float cutDirection = 0;
        public float rotation = 0;

        public Note(float time, float xPos, float yPos, float speed, TrackingPoint[] type, float cutDirection, float rotation) {
            this.time = time;
            this.speed = speed;
            this.type = type;
            this.cutDirection = cutDirection;
            this.rotation = rotation;
        }

        public Note(float time, float xPos, float yPos, TrackingPoint[] type, float cutDirection) {
            this.time = time;
            this.type = type;
            this.cutDirection = cutDirection;
        }

        public Note() {
        }
    }
}
