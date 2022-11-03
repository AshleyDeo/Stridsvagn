using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameHUD : MonoBehaviour {
	[SerializeField] GameControllerTest _controller = null;
	[SerializeField] TankBase tank;
	[SerializeField] TMP_Text ammo;
	[SerializeField] TMP_Text enemies;
	void UpdateAmmo() {
		if (tank.GetProjectile() == null) return;
		ammo.text = tank.GetProjectile().ammoName;
	}
	void UpdateEnemies() {
		if (_controller != null) {
			enemies.text = "Enemies: " + _controller.CountEnemies() + "\r\n" + "Allies: " + _controller.CountAllies();
		}
	}
	void Update() {
		UpdateAmmo();
		UpdateEnemies();
	}
}