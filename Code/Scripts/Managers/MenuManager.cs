using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    [SerializeField] private Stack<GameObject> prevMenus = new();
    public int nextScene;
	public void AddPrevMenu(GameObject menu) {
		prevMenus.Push(menu);
		//Debug.Log($"Added {prevMenus.Peek().name}");
	}
    public void GoToNextMenu(GameObject menu) {
        menu.SetActive(true);
		Debug.Log(prevMenus.Peek().name);
		prevMenus.Peek().SetActive(false);
	}
	public void GoToPrevMenu(GameObject menu) {
		GameObject prevMenu = prevMenus.Pop();
		prevMenu.SetActive(true);
		menu.SetActive(false);
	}
}