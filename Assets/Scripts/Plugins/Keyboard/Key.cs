namespace Keyboard {
	[System.Serializable]
	public class Key {
		public string keyName;
		public string character;
		public float[] position = new float[]{0,0};
		public float[] size = new float[]{0,0};
	}
}