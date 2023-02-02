using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo Type", menuName = "Assets/Ammo/Drop")]
public class DropAmmo : AmmoType {
	public override void Use(Transform loc) {
		SoundManager.Instance.PlaySound(AudioClip);
		GameObject laidMine = Instantiate(Ammo, loc.position, Quaternion.identity) as GameObject;
	}
}