using UnityEngine;

[CreateAssetMenu(fileName = "New Shield Powerup", menuName = "Assets/Powerups/Shield")]
public class Shield : PowerupEffect {
	public override void Apply(GameObject target) {
		IPickup collector = target.GetComponent<IPickup>();
		if (collector != null) {
			SoundManager.Instance.PlaySound(powerupSound);
			collector.ActivateShield(5f);
		}
		TankControllerTest test = target.GetComponent<TankControllerTest>();
		if (test != null) {
			test.ActivateShield();
		}
		TankControllerMenu menu = target.GetComponent<TankControllerMenu>();
		if (menu != null) {
			menu.GetComponent<TankControllerMenu>().ActivateShield();
		}
	}
}