namespace BeatSaber {
    public class BSObstacle {
        private float _time;
        private int _lineIndex;
        private int _type;
        private float _duration;
        private float _width;

        public Obstacle ToObstacle() {
            return new Obstacle(_time, 
                                (_lineIndex != 5 ? _lineIndex/2f - 2 : 0), 
                                (_type == 0 ? 1.5f : 2f), 
                                (_type == 0 ? 1 : 3), 
                                (_type == 0 ? 4f : 1f), 
                                _duration, 
                                _type );
        }
    }
}
