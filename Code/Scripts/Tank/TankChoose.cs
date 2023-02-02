using UnityEditor;
using UnityEngine;

public class TankChoose : TankBase {
	[SerializeField] private CharacterMainMenuTest _menu;
	public void SelectTank() {
		int index = _menu.TankSelector;
		_tank = _menu.Tanks[index];
		LoadTank();
	}
}