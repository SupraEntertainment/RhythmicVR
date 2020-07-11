using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace RhythmicVR.BeatSaber {
    /// <summary>
    /// Beat saber song converter
    /// ----
    /// converts beatsaber song files and beatmaps as a whole into RhythmicVR songs and beatmaps.
    /// Supports mapping extensions and noodle extensions
    /// </summary>
    public class SongLoader {

        public static string ConvertSong(string filePath, Core gm) {
            try {
                Song song = JsonUtility.FromJson<Song>(File.ReadAllText(filePath + Path.DirectorySeparatorChar + "info.dat"));

                RhythmicVR.Song convertedSong;
                List<RhythmicVR.Beatmap> convertedBeatmaps = new List<RhythmicVR.Beatmap>();
            
                foreach (var difficulty in song._difficultyBeatmapSets) {
                    foreach (var difficultyBeatmap in difficulty._difficultyBeatmaps) {
                    
                        var beatmapPath = filePath + Path.DirectorySeparatorChar + difficultyBeatmap._beatmapFilename;
                        string beatmapJson = File.ReadAllText(beatmapPath);
                        RhythmicVR.Beatmap bm;
                        if (difficultyBeatmap._customData._requirements.Contains("Mapping Extensions")) { //Array.IndexOf(difficultyBeatmap._customData._requirements, "Mapping Extensions") > -1) { 
                            bm = JsonUtility.FromJson<MappingExtensions.Beatmap>(beatmapJson).ToBeatmap();
                        } else if (difficultyBeatmap._customData._requirements.Contains("Noodle Extensions")) {
                            bm = JsonUtility.FromJson<NoodleExtensions.Beatmap>(beatmapJson).ToBeatmap();
                        }else {
                            bm = JsonUtility.FromJson<Beatmap>(beatmapJson).ToBeatmap();
                        }

                        foreach (var note in bm.notes) {
                            note.time = (note.time / song._beatsPerMinute) * 60;
                        }
                        convertedBeatmaps.Add(bm);
                    }
                }
                convertedSong = song.ToSong();
                var cover = File.ReadAllBytes(filePath + Path.DirectorySeparatorChar + song._coverImageFilename);
                var audio = File.ReadAllBytes(filePath + Path.DirectorySeparatorChar + song._songFilename);

                return gm.SaveSongToFile(convertedSong, convertedBeatmaps.ToArray(), cover, audio);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
