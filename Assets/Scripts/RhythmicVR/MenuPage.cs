using System.Collections.Generic;
using UnityEngine;

namespace RhythmicVR {
	public class MenuPage : Object {
		public int height;
		public string pageName;
		public GameObject buttonOnParentMenu;
		public GameObject gameObject;
		public List<MenuPage> pageChildren = new List<MenuPage>();
		public List<SettingsField> elementChildren = new List<SettingsField>();
		public MenuPage parent;

		public void AddChildPage(MenuPage menuPage) {
			pageChildren.Add(menuPage);
			var content = gameObject.transform.Find("Viewport/Content").gameObject;
			var element = Instantiate(menuPage.buttonOnParentMenu, content.transform); // instantiate UI element
			var rt = element.GetComponent<RectTransform>(); //get rect transform
			rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, -height); // set elements position
			height += (int)rt.rect.height;
			content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, height); //set height of conent element
			menuPage.buttonOnParentMenu = element;
		}

		public void AddElement(SettingsField field) {
			elementChildren.Add(field);
			var content = gameObject.transform.Find("Viewport/Content").gameObject;
			field.initializedObject = Instantiate(FindObjectOfType<GameManager>().uiManager.BuildUiElement(field.name, field.type), content.transform); // instantiate UI element
			var rt = field.initializedObject.GetComponent<RectTransform>(); //get rect transform
			rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, -height); // set elements position
			height += (int)rt.rect.height;
			content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, height); //set height of conent element
		}

		public void SetActive(bool state) {
			gameObject.SetActive(state);
		}

		public override string ToString() {
			return pageName;
		}
	}
}