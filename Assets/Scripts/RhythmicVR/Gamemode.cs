using System.Collections;
using ScoreManager;
using UnityEngine;

namespace RhythmicVR {
	/// <summary>
	/// stores info about gamemode such as the default Target Object,
	/// wich tracked devices to use and wich prefab to attach
	/// and how to manipulate the beatmap spanwing position scale, rotation
	/// </summary>
	public abstract class Gamemode : PluginBaseClass {
	
		public string gamemodeName;
		public Sprite icon;
		protected Coroutine cr;

		[Tooltip("place the object here, that has the component \"TargetObject\" or a child class of it attatched")]
		public GameObject targetObject;
		public Playthrough playthroughObject;
		public TrackedDevicePair[] trackedObjects;
		public SpaceMapping targetSpaceMapping; // translates json file positioning into unity coordinates (if needed) e.g. x=0, y=z z=y

		public override void Init(Core core) {
			base.Init(core);
			type = AssetType.Gamemode;
		}


		/// <summary>
		/// Spawn a Target Object
		/// </summary>
		/// <param name="noteData">The note data to use</param>
		public virtual void SpawnTarget(Note noteData) {
			
		}
		
		/// <summary>
		/// Start a Beatmap
		/// </summary>
		/// <param name="song">The Song to play</param>
		/// <param name="difficulty">The Difficulty, of which to take the beatmap</param>
		/// <param name="modfiers">[Not Implemented] The modifiers to use while playing</param>
		public virtual void StartBeatmap(Song song, Difficulty difficulty, string modfiers) {
			Time.timeScale = 0;
			SongType st = (SongType)core.pluginManager.Find(difficulty.type);
			var bm = st.LoadBeatmap(song.pathToDir + "/" + difficulty.beatMapPath);
			core.uiManager.InBeatmap();
			core.songIsPlaying = true;
			core.allowPause = true;
			core.currentlyPlayingSong = song;
			((ScoreManager.ScoreManager) core.pluginManager.Find("score_manager")).SelectSongAndAddPlaythrough(song);
			cr = StartCoroutine(PlayNotes(bm));
			PlaySongAudio(song);
		}

		/// <summary>
		/// Play the audio belonging to song object
		/// </summary>
		/// <param name="song">The song, who's audio to play</param>
		protected virtual void PlaySongAudio(Song song) {
			var path = song.pathToDir + song.songFile;
			core.audioSource.clip = Util.GetAudioClipFromPath(path);
			core.audioSource.Play();
			Time.timeScale = 1;
		}

		/// <summary>
		/// Play Notes coroutine, plays all notes timed properly
		/// </summary>
		/// <param name="bm">The beatmap to take the notes from</param>
		/// <returns></returns>
		protected virtual IEnumerator PlayNotes(Beatmap bm) {
			//var beatmapLength = bm.notes[bm.notes.Length-1].time;
			float currentTime = 0;
			for (var i = 0; i < bm.notes.Length; i++) {
				var note = bm.notes[i];
				Debug.Log("Wait time: " + (note.time - currentTime));
				yield return new WaitForSeconds((note.time - currentTime));
				currentTime = note.time;
				Debug.Log("Current Time: " + currentTime);
				SpawnTarget(note);
			}

			Debug.Log("Finished placing target objects");
			core.StopBeatmap(Core.BeatmapPauseReason.COMPLETED);
		}

		/// <summary>
		/// Exit the currently playing beatmap (stops the coroutine, disables pause button, hides pause menu, and returns to the menu)
		/// </summary>
		public virtual void ExitBeatmap() {
			StopCoroutine(cr);
			var scoreManager = ((ScoreManager.ScoreManager) core.pluginManager.Find("score_manager"));
			scoreManager.SaveCurrentScores();
			scoreManager.DisplayScoresOnScoreboard(core.currentlyPlayingSong);
			core.audioSource.time = 0;
			core.audioSource.clip = null;
			core.allowPause = false;
			core.isPaused = false;
			foreach (var target in FindObjectsOfType<TargetObject>()) {
				Destroy(target.gameObject);
			}

			core.uiManager.ToSongListMenu();
			core.uiManager.HidePauseMenu();
		}
	}
}