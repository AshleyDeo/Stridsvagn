using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    public int ammo;
    public PowerupEffect powerup;
    //public void OnTr(Collider collider) {
    //    if (collider.gameObject.CompareTag("Player")) {
    //        Debug.Log(collider.gameObject.name);
    //        //collider.gameObject.transform.parent.Get0Component<TankControllerTest>().Pickup(ammo);
    //        Destroy(gameObject);
    //    }
    //}
    void OnTriggerEnter2D(Collider2D collision) {
		Debug.Log(collision.transform.name);
		if (collision.gameObject.CompareTag("Player")) {
			Debug.Log(collision.transform.parent.name);
            Transform player = collision.transform;

			while (player.name != "Tank") {
                player = player.parent;
            }
            powerup.Apply(player.gameObject);
			//collider.gameObject.transform.parent.Get0Component<TankControllerTest>().Pickup(ammo);
			Destroy(gameObject);
		}

	}
}