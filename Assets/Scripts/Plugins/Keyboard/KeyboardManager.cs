using System;
using System.Collections.Generic;
using System.IO;
using Keyboard;
using RhythmicVR;
using UnityEngine;
using UnityEngine.UI;
using Event = UnityEngine.Event;

namespace Keyboard {
    
    [Serializable]
    public class KeyboardManager : PluginBaseClass {

        public GameObject keyboardCanvasPrefab;
        public GameObject keyPrefab;

        [NonSerialized]private List<GameObject> keys = new List<GameObject>();

        [NonSerialized]private GameObject keyboard;

        public List<Key> keyList = new List<Key>();


        public override void Init(Core core) {
            base.Init(core);
            keyboard = Instantiate(keyboardCanvasPrefab);
            LoadKeyboardJson(core.config.keyboardSavePath);
            InstantiateKeys();
            //SaveKeyboardJson(core.config.keyboardSavePath);
        }

        private void LoadKeyboardJson(string path) {
            if (Util.EnsureFileIntegrity(path + "keyboard-active.json")) {
                keyList.Clear();
                var keyArray = JsonUtility.FromJson<KeyList>(File.ReadAllText(path + "keyboard-active.json")).keys;
                keyList.AddRange(keyArray);
            }

            InstantiateKeys();
        }

        private void InstantiateKeys() {
            for (int i = 0; i < keyboard.transform.childCount; i++) {
                Destroy(keyboard.transform.GetChild(i).gameObject);
            }
            foreach (var key in keyList) {
                var keyObject = Instantiate(keyPrefab, keyboard.transform);
                keys.Add(keyObject);
                try {
                    keyObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(key.position[0], key.position[1]);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
                keyObject.GetComponentInChildren<Text>().text = key.keyName;
                keyObject.GetComponent<Button>().onClick.AddListener(delegate { Event.KeyboardEvent(key.character); });
            }
        }

        private void SaveKeyboardJson(string path) {
            if (Util.EnsureDirectoryIntegrity(path, true)) {
                File.WriteAllText(path + "keyboard-active.json", JsonUtility.ToJson(new KeyList {keys = keyList.ToArray()}, true));
            }
        }
    }
}

[Serializable]
internal class KeyList {
    public Key[] keys;
}
