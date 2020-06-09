using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace VRRythmGame.BeatSaber {
    public class SongLoader {

        public static string ConvertSong(string filePath, GameManager gm) {
            BSSong song = JsonUtility.FromJson<BSSong>(File.ReadAllText(filePath + "/info.dat"));

            Song convertedSong;
            List<Beatmap> convertedBeatmaps = new List<Beatmap>();
            
            foreach (var difficulty in song._difficultyBeatmapSets) {
                foreach (var difficultyBeatmap in difficulty._difficultyBeatmaps) {
                    
                    var beatmapPath = filePath + Path.DirectorySeparatorChar + difficultyBeatmap._beatmapFilename;
                    string beatmapJson = File.ReadAllText(beatmapPath);
                    Beatmap bm;
                    if (difficultyBeatmap._customData[0]._requirements.Contains("Mapping Extensions")) {
                        bm = LoadMappingExtensionsSong(beatmapJson);
                    } else if (difficultyBeatmap._customData[0]._requirements.Contains("Noodle Extensions")) {
                        bm = LoadNoodleExtensionsSong(beatmapJson);
                    }else {
                        bm = LoadDefaultSong(beatmapJson);
                    }
                    convertedBeatmaps.Add(bm);
                }
            }
            convertedSong = song.ToSong();

            return gm.SaveSongToFile(convertedSong, convertedBeatmaps.ToArray());
        }
        
        public static Beatmap LoadDefaultSong(string jsonString) {
            return JsonUtility.FromJson<BSBeatmap>(jsonString).ToBeatmap();
        }
    
        public static Beatmap LoadMappingExtensionsSong(string jsonString) {
            return JsonUtility.FromJson<BSMEBeatmap>(jsonString).ToBeatmap();
        }
    
        public static Beatmap LoadNoodleExtensionsSong(string jsonString) {
            return JsonUtility.FromJson<BSNEBeatmap>(jsonString).ToBeatmap();
        }
    }
}
