using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace VRRythmGame.BeatSaber {
    public class SongLoader {

        public static string ConvertSong(string filePath, GameManager gm) {
            try {
                Song song = JsonUtility.FromJson<Song>(File.ReadAllText(filePath + Path.DirectorySeparatorChar + "info.dat"));

                VRRythmGame.Song convertedSong;
                List<VRRythmGame.Beatmap> convertedBeatmaps = new List<VRRythmGame.Beatmap>();
            
                foreach (var difficulty in song._difficultyBeatmapSets) {
                    foreach (var difficultyBeatmap in difficulty._difficultyBeatmaps) {
                    
                        var beatmapPath = filePath + Path.DirectorySeparatorChar + difficultyBeatmap._beatmapFilename;
                        string beatmapJson = File.ReadAllText(beatmapPath);
                        VRRythmGame.Beatmap bm;
                        if (difficultyBeatmap._customData._requirements.Contains("Mapping Extensions")) { //Array.IndexOf(difficultyBeatmap._customData._requirements, "Mapping Extensions") > -1) { 
                            bm = JsonUtility.FromJson<MappingExtensions.Beatmap>(beatmapJson).ToBeatmap();
                        } else if (difficultyBeatmap._customData._requirements.Contains("Noodle Extensions")) {
                            bm = JsonUtility.FromJson<NoodleExtensions.Beatmap>(beatmapJson).ToBeatmap();
                        }else {
                            bm = JsonUtility.FromJson<Beatmap>(beatmapJson).ToBeatmap();
                        }
                        convertedBeatmaps.Add(bm);
                    }
                }
                convertedSong = song.ToSong();

                return gm.SaveSongToFile(convertedSong, convertedBeatmaps.ToArray());
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
