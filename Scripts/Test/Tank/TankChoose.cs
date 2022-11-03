using UnityEditor;
using UnityEngine;

public class TankChoose : TankBase {
	[SerializeField] private CharacterMainMenuTest _menu;
	public void SelectTank() {
		int index = _menu.tankSelector;
		_tank = _menu.tanks[index];
		LoadTank();
	}
}