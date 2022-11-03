using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour {
    CircleCollider2D _collider;

	[SerializeField] int damage;
    public Transform owner;
	[SerializeField] GameObject Explosion;

	void Awake() {
        _collider = GetComponent<CircleCollider2D>();
        _collider.enabled = false;
    }
    void Start() {
		StartCoroutine(SetColliderActive());
	}
    IEnumerator SetColliderActive() {
        yield return new WaitForSeconds(2);
        _collider.enabled = true;
        this.transform.GetChild(1).gameObject.SetActive(true);
    }
    void OnTriggerEnter2D(Collider2D collision) {
        if (_collider.enabled == false) return;
        if (collision.transform == owner) return;

		//Debug.Log(collision.gameObject.name);
		var explosionVfx = Instantiate(Explosion, this.transform.position, Quaternion.identity) as GameObject;

		IDestructible destructible = collision.gameObject.GetComponent<IDestructible>();
		if (destructible != null) { destructible.Damage(damage); }

		Destroy(explosionVfx, 5);
		Destroy(gameObject);
    }
}