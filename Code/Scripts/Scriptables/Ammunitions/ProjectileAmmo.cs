using System.Buffers;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo Type", menuName = "Assets/Ammo/Projectile")]
public class ProjectileAmmo : AmmoType {
	public float Speed;
	private Vector3 _destination;
    public override void Use(Transform loc) {
		if(loc == null) return;
		SoundManager.Instance.PlaySound(AudioClip);
		Ray ray = new(loc.position, loc.forward);
		_destination = Physics.Raycast(ray, out RaycastHit hit) ? hit.point : ray.GetPoint(1000);

		var projectileObject = Instantiate(Ammo, loc.position, loc.rotation) as GameObject;
		projectileObject.GetComponent<Projectile>().owner = loc.parent.parent;
		projectileObject.tag = loc.tag;
		projectileObject.GetComponent<Rigidbody>().velocity = (_destination - loc.position).normalized * Speed;
	}
}