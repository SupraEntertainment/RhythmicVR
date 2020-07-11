using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace RhythmicVR {
	public class SettingsManager : Object {
		public List<SettingsField> settings = new List<SettingsField>();
		public GameObject settingsMenuParent;
		private Core core;
		private List<MenuPage> allPages = new List<MenuPage>();

		public SettingsManager(Core core) {
			this.core = core;
			settingsMenuParent = core.uiManager.settingsMenu.transform.Find("Canvas").gameObject;
		}
		
		/// <summary>
		/// updates the entire settings page by rewriting it
		/// </summary>
		public void UpdateSettingsUi() {
			DeleteAllSettingsPages();
			var page = CreatePage("Settings", null);
			var backButtonGo = page.pageObject.transform.Find("Btn_back").gameObject;
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
			setting.SetupListeners();
		}

		private MenuPage CreatePage(string pageName, MenuPage parent) {
			MenuPage page = new MenuPage {pageName = pageName};
			page.buttonOnParentMenu = core.uiManager.BuildUiElement(page.pageName, UiType.Category);
			page.pageObject = Instantiate(core.uiManager.scrollList, settingsMenuParent.transform.GetChild(0)); //instatiate main settings page
			if ((object)parent != null) { // unity does weird operator magic, therefore cast to normal System.Object
				page.parent = parent;
				parent.AddChildPage(page);
				page.buttonOnParentMenu.GetComponent<Button>().onClick.AddListener(delegate { DisableAllSettingsPages(); page.SetActive(true); }); // go to this new page when clicking on category
			}
			page.pageObject.transform.Find("Btn_back").GetComponent<Button>().onClick.AddListener(delegate { DisableAllSettingsPages(); page.parent.SetActive(true); }); // back button lsitener (to activate previous page)
			allPages.Add(page); // add to page list
			return page;
		}

		public void DisableAllSettingsPages() {
			foreach (var page in allPages) {
				page.SetActive(false);
			}
		}

		private void DeleteAllSettingsPages() {
			allPages.Clear();
		}
	}
}