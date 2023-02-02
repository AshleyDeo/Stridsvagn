using System.Collections;
using UnityEngine;

public class Crate : MonoBehaviour, IDestructible {
    private Health Health;
    [SerializeField] private int healthPoints;
    [SerializeField] private GameObject broken;
    [SerializeField] private PowerupEffect[] powerups;
    [SerializeField] private Vector2 xRange;
    [SerializeField] private Vector2 zRange;
    [SerializeField] private LayerMask layerMask;
    public GameObject explosion;
    public AudioClip destroyAudio;

    public static int numCrates = 0;

    void Awake() {
		Health = gameObject.AddComponent<Health>();
        Health.SetHP(healthPoints);
    }
    void Start() {

    }
	public void Damage(int damage) {
		Health.DecreaseHP(damage);
        if (Health.HP <= 0) {
           // HP.OnDead += OnDead;
           OnDead();
        }
		//Debug.Log(HP.health);
	}
	//void OnDead(object sender, System.EventArgs e) {
	void OnDead() {
		SoundManager.Instance.PlaySound(destroyAudio);
		int chosen = Random.Range(0, powerups.Length);
        GameObject broke = Instantiate(broken, transform.position, broken.transform.rotation) as GameObject;
        broke.GetComponent<Pickup>().Powerup = powerups[chosen];
        GameObject pickup = Instantiate(powerups[chosen].Drop, transform.position + new Vector3(0,-0.15f,0), broken.transform.rotation) as GameObject;
        pickup.transform.parent = broke.transform;
        broke.transform.Rotate(180f, 0f, 0f);
        pickup.transform.Rotate(-90f, 0f, 0f);
        transform.position= new(0, -20f, 0);
        //StartCoroutine(Respawn(Random.Range(10f, 30f)));
	}
    IEnumerator Respawn(float timer) {
        yield return new WaitForSeconds(timer);
        Collider[] hits;

        Vector3 pos;
		do {
            pos = new Vector3(Random.Range(xRange.x, xRange.y), 0.2f, Random.Range(zRange.x, zRange.y));
            hits = Physics.OverlapSphere(pos, 2f, layerMask);
        }
        while (hits.Length > 0);
		transform.position = pos;
        Debug.Log($"Position Chosen: {pos}");
		Health.ResetHP();
    }
}