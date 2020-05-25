using System.ComponentModel;

[System.Serializable]
public class Difficulty {
	public string name = "Normal";
	public float difficulty = 10; 	// 0 = no notes, 10 = normal aka certain very playable NPS and simple patterns,
									// easy is inbetween 1 and 10... hard, etc are above, at like 15, 20, 25 etc.
	public string beatMapPath = "normal.json";
	public string beatMapAuthor;
	public string description;
	public string[] information = {};
	public string[] recommendedExtensions = {};
	public string[] requiredExtensions = {};

}