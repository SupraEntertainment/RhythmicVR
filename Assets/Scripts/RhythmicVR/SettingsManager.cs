using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RhythmicVR {
	public class SettingsManager : Object {
		public List<SettingsField> settings = new List<SettingsField>();
		public GameObject settingsMenuParent;
		public int defaultElementheight = 100;
		private GameManager gm;
		private List<GameObject> allPages = new List<GameObject>();

		public SettingsManager(GameManager gm) {
			this.gm = gm;
		}
		
		public void UpdateSettingsUi() {
			var mainSettingsPage = Instantiate(gm.uiManager.scrollList, settingsMenuParent.transform.GetChild(0));
			mainSettingsPage.transform.Find("Btn_back").GetComponent<Button>().onClick.AddListener(delegate { gm.uiManager.ToMainMenu(); });
			allPages.Add(mainSettingsPage);
			var content = mainSettingsPage.transform.Find("Viewport/Content").gameObject;
			content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 20 + settings.Count * (defaultElementheight + 20));
			for (var i = 0; i < settings.Count; i++) {
				RecursiveFunction(settings[i], content, i);
			}
			disableAllSettingsPages();
			mainSettingsPage.SetActive(true);
		}

		private void RecursiveFunction(SettingsField setting, GameObject parent, int index) {
			var settingUiElement = Instantiate(gm.uiManager.BuildUIElement(setting.name, setting.type), parent.transform);
			var rt = settingUiElement.GetComponent<RectTransform>();
			settingUiElement.GetComponent<RectTransform>().anchoredPosition = new Vector2(rt.anchoredPosition.x, -20 - index * (rt.rect.height+20));
			if (setting.type == UiType.Category) {
				var settingsPage = Instantiate(gm.uiManager.scrollList, settingsMenuParent.transform.GetChild(0));
				allPages.Add(settingsPage);
				settingsPage.transform.Find("Btn_back").GetComponent<Button>().onClick.AddListener(delegate { disableAllSettingsPages(); parent.transform.parent.parent.gameObject.SetActive(true); });
				settingUiElement.GetComponent<Button>().onClick.AddListener(delegate { disableAllSettingsPages(); settingsPage.SetActive(true); });
				var content = settingsPage.transform.Find("Viewport/Content").gameObject;
				content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 20 + setting.children.Length * (defaultElementheight + 20));
				for (var i = 0; i < setting.children.Length; i++) {
					RecursiveFunction(setting.children[i], content, i);
				}
			}
		}

		public void disableAllSettingsPages() {
			foreach (var page in allPages) {
				page.SetActive(false);
			}
		}
		
	}
}