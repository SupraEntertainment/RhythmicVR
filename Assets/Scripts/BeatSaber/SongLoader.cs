﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace BeatSaber {
    public class SongLoader {

        public void ConvertBeatmap(string jsonString, string filePath) {
            BSSong song = JsonUtility.FromJson<BSSong>(jsonString);

            Song convertedSong;
            List<Beatmap> convertedBeatmaps = new List<Beatmap>();
            
            foreach (var difficulty in song._difficultyBeatmapSets) {
                foreach (var difficultyBeatmap in difficulty._difficultyBeatmaps) {
                    string beatmapJson = File.ReadAllText(filePath + "/" + difficultyBeatmap._beatmapFilename);
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
        }
        
        public Beatmap LoadDefaultSong(string jsonString) {
            return JsonUtility.FromJson<BSBeatmap>(jsonString).ToBeatmap();
        }
    
        public Beatmap LoadMappingExtensionsSong(string jsonString) {
            return JsonUtility.FromJson<BSMEBeatmap>(jsonString).ToBeatmap();
        }
    
        public Beatmap LoadNoodleExtensionsSong(string jsonString) {
            return JsonUtility.FromJson<BSNEBeatmap>(jsonString).ToBeatmap();
        }
    }
}
