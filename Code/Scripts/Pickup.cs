using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    public PowerupEffect Powerup;
	public int Ammo;
    void OnTriggerEnter(Collider collision) {
		GameObject target = collision.transform.gameObject;
		IPickup collector = target.GetComponent<IPickup>();
		if (collector != null) {
			Powerup.Apply(target);
			Destroy(gameObject);
		}
	}
}