using System.Collections;
using BlockSong;
using RhythmicVR;
using UnityEngine;
using Beatmap = BlockSong.Beatmap;

namespace BeatPunchGamemode {
	public class BeatPunchGamemode : Gamemode {

		/// <summary>
		/// Spawn a Target Object
		/// </summary>
		/// <param name="noteData">The note data to use</param>
		public void SpawnTarget(BlockSong.Note noteData) {
			noteData.xPos = noteData.xPos / 2;
			noteData.yPos = noteData.yPos / 2;
			GameObject cube = Instantiate(targetObject, new Vector3(noteData.xPos, noteData.yPos, core.spawnDistance), new Quaternion(0, 0, noteData.cutDirection, 0));
			cube.GetComponent<BlockSongTargetObject>().InitNote(noteData);
		}

		// spawn a obstacle ^ same here
		public void SpawnObstacle(float speed, float xCoord, float yCoord, float viewRotation, float playspaceRoation, float width, float height) {
        
		}
		
		/// <summary>
		/// Start a Beatmap
		/// </summary>
		/// <param name="song">The Song to play</param>
		/// <param name="difficulty">The Difficulty, of which to take the beatmap</param>
		/// <param name="modfiers">[Not Implemented] The modifiers to use while playing</param>
		public override void StartBeatmap(Song song, Difficulty difficulty, string modfiers) {
			Time.timeScale = 0;
			BlockSong.BlockSong st = (BlockSong.BlockSong)core.pluginManager.Find(difficulty.type);
			Beatmap bm = st.LoadBeatmap(song.pathToDir + "/" + difficulty.beatMapPath);
			core.uiManager.InBeatmap();
			core.songIsPlaying = true;
			core.allowPause = true;
			core.currentlyPlayingSong = song;
			((ScoreManager.ScoreManager) core.pluginManager.Find("score_manager")).SelectSongAndAddPlaythrough(song);
			cr = StartCoroutine(PlayNotes(bm, song.startTimeOffset));
			PlaySongAudio(song);
		}

		/// <summary>
		/// Play Notes coroutine, plays all notes timed properly
		/// </summary>
		/// <param name="bm">The beatmap to take the notes from</param>
		/// <param name="startOffset">time in seconds to offset the note playing</param>
		/// <returns></returns>
		protected IEnumerator PlayNotes(Beatmap bm, float startOffset) {
			//var beatmapLength = bm.notes[bm.notes.Length-1].time;
			float currentTime = startOffset;
			for (var i = 0; i < bm.notes.Length; i++) {
				BlockSong.Note note = bm.notes[i];
				Debug.Log("Wait time: " + (note.time - currentTime));
				yield return new WaitForSeconds((note.time - currentTime));
				currentTime = note.time;
				Debug.Log("Current Time: " + currentTime);
				SpawnTarget(note);
			}

			Debug.Log("Finished placing target objects");
			core.StopBeatmap(Core.BeatmapPauseReason.COMPLETED);
		}
	}
}