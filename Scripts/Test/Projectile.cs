using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Projectile : MonoBehaviour {
	Rigidbody2D rb;

	public Transform owner;
	[SerializeField] int damage;

	[SerializeField] GameObject Impact;
	LayerMask layerMask;
	Vector3 lastVelocity;
	void Awake() {
		rb = GetComponent<Rigidbody2D>();
		layerMask = LayerMask.GetMask("ShieldMask");
	}
	void Update() {
		lastVelocity = rb.velocity;
	}
	void OnCollisionEnter2D(Collision2D collision) {
		Debug.Log(collision.gameObject.name);
		Ray ray = new(transform.position, transform.up);
		if (Physics.Raycast(ray, out RaycastHit hit, Time.deltaTime * 10f, 10)) {
			Vector3 dir = Vector3.Reflect(ray.direction, hit.normal);
			float rot = 90 - Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
			transform.eulerAngles = new Vector3(0, rot, 0);
			Debug.Log("Ricochet");
		}
		if (collision.transform == owner) return;
		if (collision.gameObject.layer == LayerMask.NameToLayer("ShieldMask")) return;

		var impactVfx = Instantiate(Impact, collision.contacts[0].point, Quaternion.identity) as GameObject;

		IDestructible destructible = collision.gameObject.GetComponent<IDestructible>();
		if (destructible != null) { destructible.Damage(damage); }

		Destroy(impactVfx, 5);
		Destroy(gameObject);
	}
}