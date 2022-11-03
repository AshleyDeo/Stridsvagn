using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    public PowerupEffect powerup;
	public int ammo;
    void OnTriggerEnter2D(Collider2D collision) {
		GameObject target = collision.transform.gameObject;
		IPickup collector = target.GetComponent<IPickup>();
		if (collector != null) {
			powerup.Apply(target);
			Destroy(gameObject);
		}
	}
}