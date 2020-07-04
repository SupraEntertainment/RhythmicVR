using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace RhythmicVR {
	public class SettingsManager : Object {
		public List<SettingsField> settings = new List<SettingsField>();
		public GameObject settingsMenuParent;
		private GameManager gm;
		private List<MenuPage> allPages = new List<MenuPage>();

		public SettingsManager(GameManager gm) {
			this.gm = gm;
		}
		
		/// <summary>
		/// updates the entire settings page by rewriting it
		/// </summary>
		public void UpdateSettingsUi() {
			DeleteAllSettingsPages();
			var page = CreatePage("Settings", null);
			var backButtonGo = page.gameObject.transform.Find("Btn_back").gameObject;
			Destroy(backButtonGo); //.GetComponent<Button>().onClick.AddListener( delegate { Debug.Log("Back to main menu button pressed"); gm.uiManager.ToMainMenu(); }); // set back button callback
			//var content = page.gameObject.transform.Find("Viewport/Content").gameObject; // set content element
			//int contentHeight = 0;
			for (var i = 0; i < settings.Count; i++) { // go through all elements inside this element
				SetupElement(settings[i], page);
			}
			//content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, contentHeight); //set height of conent element
			
			// activate main settings page
			DisableAllSettingsPages();
			page.SetActive(true);
		}

		private void SetupElement(SettingsField setting, MenuPage parent) {
			string[] path = setting.menuPath.Split('/');
			MenuPage currentPage = parent;
			foreach (var page in path) {
				bool pageExists = false;
				foreach (var menuPage in allPages) {
					if (string.Equals(page, menuPage.pageName, StringComparison.CurrentCultureIgnoreCase)) {
						pageExists = true;
						currentPage = menuPage;
					}
				}
				if (!pageExists) {
					currentPage = CreatePage(page, currentPage);
				}
			}
			currentPage.AddElement(setting);
		}

		private MenuPage CreatePage(string pageName, MenuPage parent) {
			MenuPage page = new MenuPage {pageName = pageName};
			page.buttonOnParentMenu = gm.uiManager.BuildUiElement(page.pageName, UiType.Category);
			page.gameObject = Instantiate(gm.uiManager.scrollList, settingsMenuParent.transform.GetChild(0)); //instatiate main settings page
			if ((object)parent != null) { // unity does weird operator magic, therefore cast to normal System.Object
				page.parent = parent;
				parent.AddChildPage(page);
				page.buttonOnParentMenu.GetComponent<Button>().onClick.AddListener(delegate { DisableAllSettingsPages(); page.SetActive(true); }); // go to this new page when clicking on category
			}
			page.gameObject.transform.Find("Btn_back").GetComponent<Button>().onClick.AddListener(delegate { DisableAllSettingsPages(); page.parent.SetActive(true); }); // back button lsitener (to activate previous page)
			allPages.Add(page); // add to page list
			return page;
		}
		
		/// <summary>
		/// Initialize settings menu, run over this for each element, calls itself if elements are inside eachother (for categories)
		/// </summary>
		/// <param name="setting">The settings element to iterate over in this run</param>
		/// <param name="parent">The parent gameobject to initialize the element in</param>
		/// <param name="heightOfExistingElements">height positioning Offset for this element</param>
		/// <returns>Height of this iterations element</returns>
		/// 
		[Obsolete("This should not be used to create Ui elements anymore, use CreatePage() and SetupElement() instead")]
		private int InitializeUiElement(SettingsField setting, GameObject parent, int heightOfExistingElements) {
			var settingUiElement = Instantiate(gm.uiManager.BuildUiElement(setting.name, setting.type), parent.transform); // instantiate UI element
			var rt = settingUiElement.GetComponent<RectTransform>(); //get rect transform
			rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, -heightOfExistingElements); // set elements position
			if (setting.type == UiType.Category) {
				var settingsPage = Instantiate(gm.uiManager.scrollList, settingsMenuParent.transform.GetChild(0)); // instantiate category page
				//allPages.Add(settingsPage); // add page to list
				//settingsPage.transform.Find("Btn_back").GetComponent<Button>().onClick.RemoveAllListeners(); //remove all previous listeners
				settingsPage.transform.Find("Btn_back").GetComponent<Button>().onClick.AddListener(delegate { DisableAllSettingsPages(); parent.transform.parent.parent.gameObject.SetActive(true); }); // back button lsitener (to activate previous page)
				settingUiElement.GetComponent<Button>().onClick.AddListener(delegate { DisableAllSettingsPages(); settingsPage.SetActive(true); }); // go to this new page when clicking on category
				var content = settingsPage.transform.Find("Viewport/Content").gameObject; // set content element
				int contentHeight = 0;
				/*for (var i = 0; i < setting.children.Length; i++) {
					contentHeight += InitializeUiElement(setting.children[i], content, contentHeight); // create next elements
				}*/
				content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, contentHeight); //set height of conent element
			} else {
				var allInputElements = settingUiElement.GetComponentsInChildren<InputField>();
				for (var i = 0; i < allInputElements.Length; i++) {
					var inputElement = allInputElements[i];
					inputElement.onValueChanged.AddListener(delegate(string arg0) { setting.InvokeEvent(i, arg0); });
				}

				var allSliders = settingUiElement.GetComponentsInChildren<Slider>();
				foreach (var slider in allSliders) {
					slider.onValueChanged.AddListener(delegate(float arg0) { setting.InvokeEvent(0, arg0.ToString()); });
				}
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
				Destroy(page.gameObject);
			}
		}
	}
}