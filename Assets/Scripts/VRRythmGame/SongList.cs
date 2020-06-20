using System;
using System.Collections.Generic;

namespace VRRythmGame {
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

		public List<Song> GetSongsWithBeatmapsOfGamemode(string gamemodeName) {
			List<Song> localSongList = new List<Song>();
			foreach (var song in _songlist) {
				Song localSong = song;
				List<Difficulty> localDifficulties = new List<Difficulty>();
				foreach (var difficulty in song.difficulties) {
					if (difficulty.gamemode == gamemodeName) {
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