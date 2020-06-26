using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RhythmicVR {
	public class SettingsManager : Object {
		public List<SettingsField> settings = new List<SettingsField>();
		public GameObject settingsMenuParent;
		private GameManager gm;
		private List<GameObject> allPages = new List<GameObject>();

		public SettingsManager(GameManager gm) {
			this.gm = gm;
		}
		
		/// <summary>
		/// updates the entire settings page by rewriting it
		/// </summary>
		public void UpdateSettingsUi() {
			DeleteAllSettingsPages();
			var mainSettingsPage = Instantiate(gm.uiManager.scrollList, settingsMenuParent.transform.GetChild(0)); //instatiate main settings page
			mainSettingsPage.transform.Find("Btn_back").GetComponent<Button>().onClick.AddListener( delegate { Debug.Log("Back to main menu button pressed"); gm.uiManager.ToMainMenu(); }); // set back button callback
			allPages.Add(mainSettingsPage); // add to pages list
			var content = mainSettingsPage.transform.Find("Viewport/Content").gameObject; // set content element
			int contentHeight = 0;
			for (var i = 0; i < settings.Count; i++) { // go through all elements inside this element
				contentHeight += InitializeUiElement(settings[i], content, contentHeight);
			}
			content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, contentHeight); //set height of conent element
			
			// activate main settings page
			DisableAllSettingsPages();
			mainSettingsPage.SetActive(true);
		}

		/// <summary>
		/// Initialize settings menu, run over this for each element, calls itself if elements are inside eachother (for categories)
		/// </summary>
		/// <param name="setting">The settings element to iterate over in this run</param>
		/// <param name="parent">The parent gameobject to initialize the element in</param>
		/// <param name="heightOfExistingElements">height positioning Offset for this element</param>
		/// <returns>Height of this iterations element</returns>
		private int InitializeUiElement(SettingsField setting, GameObject parent, int heightOfExistingElements) {
			var settingUiElement = Instantiate(gm.uiManager.BuildUIElement(setting.name, setting.type), parent.transform); // instantiate UI element
			var rt = settingUiElement.GetComponent<RectTransform>(); //get rect transform
			rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, -heightOfExistingElements); // set elements position
			if (setting.type == UiType.Category) {
				var settingsPage = Instantiate(gm.uiManager.scrollList, settingsMenuParent.transform.GetChild(0)); // instantiate category page
				allPages.Add(settingsPage); // add page to list
				//settingsPage.transform.Find("Btn_back").GetComponent<Button>().onClick.RemoveAllListeners(); //remove all previous listeners
				settingsPage.transform.Find("Btn_back").GetComponent<Button>().onClick.AddListener(delegate { DisableAllSettingsPages(); parent.transform.parent.parent.gameObject.SetActive(true); }); // back button lsitener (to activate previous page)
				settingUiElement.GetComponent<Button>().onClick.AddListener(delegate { DisableAllSettingsPages(); settingsPage.SetActive(true); }); // go to this new page when clicking on category
				var content = settingsPage.transform.Find("Viewport/Content").gameObject; // set content element
				int contentHeight = 0;
				for (var i = 0; i < setting.children.Length; i++) {
					contentHeight += InitializeUiElement(setting.children[i], content, contentHeight); // create next elements
				}
				content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, contentHeight); //set height of conent element
			}
			return (int)rt.rect.height;
		}

		public void DisableAllSettingsPages() {
			foreach (var page in allPages) {
				page.SetActive(false);
			}
		}

		private void DeleteAllSettingsPages() {
			foreach (var page in allPages) {
				Destroy(page);
			}
		}
	}
}