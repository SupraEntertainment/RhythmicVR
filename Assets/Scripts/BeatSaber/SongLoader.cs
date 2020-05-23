using UnityEngine;

namespace BeatSaber {
    public class SongLoader {
        public Song LoadDefaultSong(string jsonString) {
            return JsonUtility.FromJson<BSSong>(jsonString).ToSong();
        }
    
        public Song LoadMappingExtensionsSong(string jsonString) {
            return JsonUtility.FromJson<BSMESong>(jsonString).ToSong();
        }
    
        public Song LoadNoodleExtensionsSong(string jsonString) {
            return JsonUtility.FromJson<BSNESong>(jsonString).ToSong();
        }
    }
}
