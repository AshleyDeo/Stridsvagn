using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo Powerup", menuName = "Assets/Powerups/Ammo")]
public class Ammo : PowerupEffect {
	public GameObject AmmoT;
	public AmmoType _Ammo;
	[Header("Stats")]
	public int AmmoAmount;
	public override void Apply(GameObject target) {
		IPickup collector = target.GetComponent<IPickup>();
		if (collector != null) {
			SoundManager.Instance.PlaySound(PowerupSound);
			collector.AddAmmo(_Ammo, AmmoAmount);
			return;
		}
	}
}