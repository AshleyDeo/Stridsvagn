using UnityEngine;

public class Projectile : MonoBehaviour {
	Rigidbody rb;

	public Transform owner;
	[SerializeField] int damage;
	[SerializeField] private LayerMask layerMask;
	[SerializeField] GameObject Impact;

	[Header("Debug")]
	[SerializeField] bool showLogs;
	void Awake() {
		rb = GetComponent<Rigidbody>();
	}
	void OnCollisionEnter(Collision collision) {
		Log(collision.gameObject.name);
		Ray ray = new(transform.position, transform.forward);
		if (Physics.Raycast(ray, out RaycastHit hit, Time.deltaTime * 10f, 10)) {
			Vector3 dir = Vector3.Reflect(ray.direction, hit.normal);
			float rot = 90 - Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
			transform.eulerAngles = new Vector3(0, rot, 0);
			Log("Ricochet");
		}
		if (collision.transform == owner) return;
		if (collision.gameObject.layer == layerMask) return;

		var impactVfx = Instantiate(Impact, collision.contacts[0].point, Quaternion.identity) as GameObject;

		IDestructible destructible = collision.gameObject.GetComponent<IDestructible>();
		if (destructible != null) { destructible.Damage(damage); }

		Destroy(impactVfx, 5);
		Destroy(gameObject);
	}
	void Log(object message) {
		if (!showLogs) return;
		Debug.Log(message);
	}
}