using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class MineNetwork : NetworkBehaviour {
    SphereCollider _collider = null;

	[SerializeField] int _damage;
    public Transform Owner;
	[SerializeField] GameObject _explosion;

	[Header("Debug")]
	[SerializeField] bool showLogs;
	[SerializeField] bool showGizmos;

	void Awake() {
        _collider = GetComponent<SphereCollider>();
        _collider.enabled = false;
    }
    void Start() {
		StartCoroutine(SetColliderActive());
	}
    IEnumerator SetColliderActive() {
        yield return new WaitForSeconds(2);
        _collider.enabled = true;
        //this.transform.GetChild(1).gameObject.SetActive(true);
    }
    void OnTriggerEnter(Collider collision) {
        if (!_collider.enabled) return;
        if (collision.transform == Owner) return;

		Log(collision.gameObject.name);
		var explosionVfx = Instantiate(_explosion, this.transform.position, Quaternion.identity) as GameObject;

		IDestructible destructible = collision.gameObject.GetComponent<IDestructible>();
		if (destructible != null) { destructible.Damage(_damage); }

		Destroy(explosionVfx, 5);
		Destroy(gameObject);
    }
    void OnDrawGizmos() {
        if(!showGizmos) return;
		if (_collider == null) return;
		Gizmos.color = new Color(1f,0f,0f,0.5f);
		Gizmos.DrawSphere(transform.position, _collider.radius);
	}
	void Log(object message) {
		if (!showLogs) return;
		Debug.Log(message);
	}
}