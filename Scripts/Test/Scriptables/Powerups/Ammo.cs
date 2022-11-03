using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo Powerup", menuName = "Assets/Powerups/Ammo")]
public class Ammo : PowerupEffect {
	public GameObject AmmoT;
	public AmmoType ammo;
	[Header("Stats")]
	public int ammoAmount;
	public override void Apply(GameObject target) {
		IPickup collector = target.GetComponent<IPickup>();
		if (collector != null) {
			SoundManager.Instance.PlaySound(powerupSound);
			collector.AddAmmo(ammo, ammoAmount);
			return;
		}
	}
}