using System.Buffers;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo Type", menuName = "Assets/Ammo/Projectile")]
public class ProjectileAmmo : AmmoType {
	public float speed;
	private Vector3 destination;
    public override void Use(Transform loc) {
		if(loc == null) return;
		SoundManager.Instance.PlaySound(audioClip);
		Ray ray = new(loc.position, loc.up);
		if (Physics.Raycast(ray, out RaycastHit hit)) {
			destination = hit.point;
		}
		else {
			destination = ray.GetPoint(10000);
		}
		var projectileObject = Instantiate(Ammo, loc.position, loc.rotation) as GameObject;
		projectileObject.GetComponent<Projectile>().owner = loc.parent.parent;
		projectileObject.GetComponent<Rigidbody2D>().velocity = (destination - loc.position).normalized * speed;
	}
}