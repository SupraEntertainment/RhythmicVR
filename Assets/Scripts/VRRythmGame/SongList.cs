using System;
using System.Collections.Generic;

namespace VRRythmGame {
	public class SongList {
		private List<Song> _songlist = new List<Song>();

		public void AddSongs(List<Song> songs) {
			_songlist.AddRange(songs);
		}

		public void Clear() {
			_songlist.Clear();
		}

		public List<Song> GetAllSongs() {
			return _songlist;
		}

		public List<Song> GetSongsWithBeatmapsOfGamemode(string gamemodeName) {
			List<Song> localsonglist = new List<Song>();
			foreach (var song in _songlist) {
				Song localsong = song;
				List<Difficulty> localdifficulties = new List<Difficulty>();
				foreach (var difficulty in song.difficulties) {
					if (difficulty.gamemode == gamemodeName) {
						localdifficulties.Add(difficulty);
					}
				}

				if (localdifficulties.Count >= 0) {
					localsong.difficulties = localdifficulties.ToArray();
					localsonglist.Add(localsong);
				}
			}
			return _songlist;
		}

	}
}