using System;
using System.Collections.Generic;

namespace RhythmicVR {
	/// <summary>
	/// Stores all loaded songs
	/// </summary>
	public class SongList {
		private List<Song> _songlist = new List<Song>();

		public void Add(Song song) {
			_songlist.Add(song);
		}

		public void AddRange(List<Song> songs) {
			_songlist.AddRange(songs);
		}

		public void Clear() {
			_songlist.Clear();
		}

		public List<Song> GetAllSongs() {
			return _songlist;
		}

		/// <summary>
		/// Returns song Objects of all songs, that have beatmaps of gamemode "gamemodeName",
		/// only with said beatmaps in them, all others removed.
		/// </summary>
		/// <param name="gamemodeName">The gamemode to filter after</param>
		/// <returns>List of songs with only beatmaps of gamemode</returns>
		public List<Song> GetSongsWithBeatmapsOfGamemode(string gamemodeName) {
			List<Song> localSongList = new List<Song>();
			foreach (var song in _songlist) {
				Song localSong = song;
				List<Difficulty> localDifficulties = new List<Difficulty>();
				foreach (var difficulty in song.difficulties) {
					if (difficulty.type == gamemodeName) {
						localDifficulties.Add(difficulty);
					}
				}

				if (localDifficulties.Count >= 0) {
					localSong.difficulties = localDifficulties.ToArray();
					localSongList.Add(localSong);
				}
			}
			return localSongList;
		}

	}
}