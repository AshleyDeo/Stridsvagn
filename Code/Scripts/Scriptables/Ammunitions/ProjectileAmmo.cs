using System.Buffers;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo Type", menuName = "Assets/Ammo/Projectile")]
public class ProjectileAmmo : AmmoType {
	public float Speed;
	private Vector3 _destination;
	public override void Use(Transform loc) {
		if (loc == null) return;
		SoundManager.Instance.PlaySound(AudioClip, 1f);
		Ray ray = new(loc.position, loc.forward);
		_destination = Physics.Raycast(ray, out RaycastHit hit) ? hit.point : ray.GetPoint(1000);

		var projectileObject = Instantiate(Ammo, loc.position, loc.rotation) as GameObject;
		projectileObject.tag = loc.tag;
		Vector3 velocity = (_destination - loc.position).normalized * Speed;
		Projectile p = projectileObject.GetComponent<Projectile>();
		if (p != null) {
			p.owner = loc.parent.parent;
			projectileObject.GetComponent<Rigidbody>().velocity = velocity;
		}
		else {
			ProjectileNetwork _p = projectileObject.GetComponent<ProjectileNetwork>();
			_p.Owner = loc.parent.parent;
			_p.SetVelocity(velocity);
		}
	}
}