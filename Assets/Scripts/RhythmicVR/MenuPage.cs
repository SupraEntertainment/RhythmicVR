using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RhythmicVR {
	public class MenuPage : Object {
		public int height;
		public string pageName;
		public GameObject buttonOnParentMenu;
		public GameObject pageObject;
		public List<MenuPage> pageChildren = new List<MenuPage>();
		public List<SettingsField> elementChildren = new List<SettingsField>();
		public MenuPage parent;

		public void AddChildPage(MenuPage menuPage) {
			pageChildren.Add(menuPage);
			var content = pageObject.transform.Find("Viewport/Content").gameObject;
			var element = Instantiate(menuPage.buttonOnParentMenu, content.transform); // instantiate UI element
			var rt = element.GetComponent<RectTransform>(); //get rect transform
			rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, -height); // set elements position
			height += (int)rt.rect.height;
			content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, height); //set height of conent element
			menuPage.buttonOnParentMenu = element;
		}

		public void AddElement(SettingsField field) {
			elementChildren.Add(field);
			var content = pageObject.transform.Find("Viewport/Content").gameObject;
			field.initializedObject = Instantiate(FindObjectOfType<Core>().uiManager.BuildUiElement(field.name, field.type), content.transform); // instantiate UI element
			var rt = field.initializedObject.GetComponent<RectTransform>(); //get rect transform
			rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, -height); // set elements position
			height += (int)rt.rect.height;
			content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, height); //set height of conent element
			if (field.type == UiType.Float || field.type == UiType.Int) {
				var slider = field.initializedObject.GetComponentInChildren<Slider>();
				slider.maxValue = field.maxValue;
				slider.minValue = field.minValue;
			}
		}

		public void SetActive(bool state) {
			pageObject.SetActive(state);
		}

		public override string ToString() {
			return pageName;
		}
	}
}