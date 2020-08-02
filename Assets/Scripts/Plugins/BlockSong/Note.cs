using RhythmicVR;

namespace BlockSong {
    [System.Serializable]
    public class Note {
        public float time = 1;
        public float xPos = 1;
        public float yPos = 0;
        public float speed = 1;
        public TrackingPoint[] target = new TrackingPoint[]{};
        public float cutDirection = 0;
        public float rotation = 0;
        public int type; // things like directional, two directional, omnidirectional

        public Note() {
        }
    }
}
