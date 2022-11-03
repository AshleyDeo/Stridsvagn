using UnityEngine;
using TMPro;

public class GameHUD : MonoBehaviour {
	[SerializeField] GameControllerTest _controller = null;
	[SerializeField] TankBase tank;
	[SerializeField] TMP_Text ammo;
	[SerializeField] TMP_Text allies;
	[SerializeField] TMP_Text enemies;
	void Update() {
		UpdateAmmo();
		UpdateAllies();
		UpdateEnemies();
	}
	void UpdateAmmo() {
		if (tank.GetProjectile() == null) return;
		ammo.text = tank.GetProjectile().ammoName;
	}
	void UpdateAllies() {
		if (_controller == null) return;
		allies.text = $"Allies: {_controller.CountAllies()}";
	}
	void UpdateEnemies() {
		if (_controller == null) return;
		enemies.text = $"Enemies: {_controller.CountEnemies()}";
	}
}