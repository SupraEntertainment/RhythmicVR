[System.Serializable]
public class Note {
    public float time;
    public float xPos;
    public float yPos;
    public float speed;
    public int type;
    public float cutDirection;
    public float rotation;

    public Note(float time = 1, float xPos = 1, float yPos = 0, float speed = 0.1f, int type = 0, float cutDirection = 0, float rotation = 0) {
        this.time = time;
        this.speed = speed;
        this.type = type;
        this.cutDirection = cutDirection;
        this.rotation = rotation;
    }
}
