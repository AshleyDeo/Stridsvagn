using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Crate : MonoBehaviour, IDestructible {
    private Health Health;
    [SerializeField] private int healthPoints;
    [SerializeField] private GameObject broken;
    [SerializeField] private GameObject[] pickups;
    [SerializeField] private PowerupEffect[] powerups;
    public GameObject explosion;
    public AudioClip destroyAudio;

    public static int numCrates = 0;

    void Awake() {
		Health = gameObject.AddComponent<Health>();
        Health.SetHP(healthPoints);
        numCrates += 1;
    }
	public void Damage(int damage) {
		Health.DecreaseHP(damage);
        if (Health.HP <= 0) {
           // HP.OnDead += OnDead;
           OnDead();
           Destroy(gameObject);
        }
		//Debug.Log(HP.health);
	}
	//void OnDead(object sender, System.EventArgs e) {
	void OnDead() {
        numCrates -= 1;
		SoundManager.Instance.PlaySound(destroyAudio);
		int chosen = Random.Range(0, powerups.Length);
        GameObject broke = Instantiate(broken, transform.position, broken.transform.rotation) as GameObject;
        broke.GetComponent<Pickup>().powerup = powerups[chosen];
        GameObject pickup = Instantiate(powerups[chosen].Drop, transform.position, transform.rotation) as GameObject;
        pickup.transform.parent = broke.transform;
        broke.transform.Rotate(180f, 0f, 0f);
	}
}