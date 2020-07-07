using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace RhythmicVR {
	/// <summary>
	/// Utility Methods, useful everywhere
	/// </summary>
	public class Util {
		
		private static IEnumerator _iterator;

		public static AudioClip audioClip; //to store the audio clip output from WebRequest handler in
		
        /// <summary>
        /// Load sprite from file path
        /// </summary>
        /// <param name="filePath">path to sprite</param>
        /// <returns>Sprite</returns>
		public static Sprite LoadSprite(string filePath) {
 
			// Load a PNG or JPG file from disk to a Sprite
			// Returns null if load fails
			
			var Tex2D = LoadTexture(filePath);
			if (Tex2D)
				return Sprite.Create(Tex2D, new Rect(0, 0, Tex2D.width, Tex2D.height),new Vector2(0,0), 100);
			return null;
		}
        
        /// <summary>
        /// Load Texture2D from file path
        /// </summary>
        /// <param name="filePath">path to Texture</param>
        /// <returns>Texture2D</returns>
		public static Texture2D LoadTexture(string filePath) {
 
			// Load a PNG or JPG file from disk to a Texture2D
			// Returns null if load fails

			//Debug.Log("loading image " + filePath);
 
			Texture2D Tex2D;
			byte[] FileData;
 
			if (File.Exists(filePath)){
				FileData = File.ReadAllBytes(filePath);
				Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
				if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
					return Tex2D;                 // If data = readable -> return texture
			}  
			return null;                     // Return null if load failed
		}

        public IEnumerator SetAudioClipFromPath(string path) {
	        Debug.Log(File.Exists(path));
	        var fullpath = "file://" + path;
	        var request = UnityWebRequestMultimedia.GetAudioClip(fullpath, AudioType.OGGVORBIS);
	        yield return request.SendWebRequest();
	        while (!request.isDone) {
		        Debug.Log("Not done yet");
	        }
	        audioClip = DownloadHandlerAudioClip.GetContent(request);
        }

        public static AudioClip GetAudioClipFromPath(string path) {
	        var util = new Util();
	        while (audioClip == null) {
		        if (_iterator == null)
		        {
			        Debug.Log("Starting DoSomething()");
			        _iterator = util.SetAudioClipFromPath(path);
		        }
 
		        if (!_iterator.MoveNext())
		        {
			        Debug.Log("DoSomething completed!");
			        _iterator = null;
		        }
	        }
	        return audioClip;
        }

        public static bool EnsureDirectoryIntegrity(string path, bool shouldCreate = false) {
	        if (!Directory.Exists(path)) {
		        if (shouldCreate) {
			        Directory.CreateDirectory(path);
			        return true;
		        }
	        }
	        return false;
        }
	}
}