namespace RhythmicVR {
	public class VolumeManager {
		private GameManager gm;

		public VolumeManager(GameManager gm) {
			this.gm = gm;
		}

		public void SetGeneralVolume(int volume) {
			gm.config.generalVolume = volume;
			gm.config.Save();
		}

		public void SetMenuVolume(int volume) {
			gm.config.menuVolume = volume;
			gm.config.Save();
		}

		public void SetSongVolume(int volume) {
			gm.config.songVolume = volume;
			gm.config.Save();
		}

		public void SetSongPreviewVolume(int volume) {
			gm.config.songPreviewVolume = volume;
			gm.config.Save();
		}

		public void SetHitVolume(int volume) {
			gm.config.hitVolume = volume;
			gm.config.Save();
		}

		public void SetMissVolume(int volume) {
			gm.config.missVolume = volume;
			gm.config.Save();
		}

		public void SetWrongHitVolume(int volume) {
			gm.config.wrongHitVolume = volume;
			gm.config.Save();
		}
	}
}