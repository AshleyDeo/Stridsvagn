using UnityEngine;
using Unity.Netcode;

public class ProjectileNetwork : NetworkBehaviour {
	public Transform Owner;
	[SerializeField] int _damage;
	[SerializeField] private LayerMask _layerMask;
	[SerializeField] GameObject _impact;
	private Rigidbody _rb;
	private float _velocity = 0;

	[Header("Debug")]
	[SerializeField] bool showLogs;
	void Awake() {
		_rb = GetComponent<Rigidbody>();
	}
	void Update() {
		if (_velocity != 0) { 
			if (_rb.velocity.magnitude < _velocity){
				Destroy(gameObject);
			}
		}
	}
	void OnCollisionEnter(Collision collision) {
		Log(collision.gameObject.name);
		Ray ray = new(transform.position, transform.forward);
		if (Physics.Raycast(ray, out RaycastHit hit, Time.deltaTime * 10f, 9)) {
			Vector3 dir = Vector3.Reflect(ray.direction, hit.normal);
			float rot = 90 - Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
			transform.eulerAngles = new Vector3(0, rot, 0);
			Log("Ricochet");
		}
		if (collision.transform == Owner) return;
		if (collision.gameObject.layer == _layerMask) return;

		var impactVfx = Instantiate(_impact, collision.contacts[0].point, Quaternion.identity) as GameObject;

		IDestructible destructible = collision.gameObject.GetComponent<IDestructible>();
		if (destructible != null) { destructible.Damage(_damage); }

		Destroy(impactVfx, 5);
		Destroy(gameObject);
	}
	public void SetVelocity(Vector3 velocity) {
			_rb.velocity = velocity;
			_velocity = (velocity.magnitude) / 2f;
	}
	void Log(object message) {
		if (!showLogs) return;
		Debug.Log(message);
	}
}