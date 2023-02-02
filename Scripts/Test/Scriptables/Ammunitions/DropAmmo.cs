using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo Type", menuName = "Assets/Ammo/Drop")]
public class DropAmmo : AmmoType {
	public override void Use(Transform loc) {
		SoundManager.Instance.PlaySound(audioClip);
		GameObject laidMine = Instantiate(Ammo, loc, false) as GameObject;
		laidMine.transform.parent = null;
	}
}