using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallDestructibleScript : MonoBehaviour, IDestructible {
	[SerializeField] private Health Health = null;
	[SerializeField] private int healthPoints;
	[SerializeField] private GameObject explosion;
	[SerializeField] private GameObject remains;

	void Start() {
		if (Health == null) {
			Health = gameObject.AddComponent<Health>();
		}
		Health.SetHP(healthPoints);
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
	void OnDead() {
		GameObject dead = Instantiate(remains, transform.position, Quaternion.identity) as GameObject;
		dead.transform.Rotate(180f, 0f, 0f);
		GameObject exp = Instantiate(explosion, transform.position, transform.rotation) as GameObject;

		Destroy(gameObject);
	}
	//   private void OnTriggerEnter2D(Collider2D collision) {
	//       if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("EnemyBullet")) {
	//           Destroy(collision.gameObject);

	//           if (HP > 1) HP--;
	//           else {
	//               HP = 0;
	//               GameObject exp = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
	//               exp.transform.parent = null;

	//               Destroy(GetComponent<BoxCollider2D>());

	//               this.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
	//               this.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
	//           }
	//       }
	//   }
	//void OnGUI()
	//{
	//    Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);

	//    // draw health bar background
	//    GUI.color = Color.grey;
	//    GUI.DrawTexture(new Rect(pos.x - 26, Screen.height - pos.y + 20, 52, 7), Box);

	//    // draw health bar amount
	//    if (HP > (initialHP / 2)) GUI.color = Color.green;
	//    if (HP <= (initialHP / 2)) GUI.color = Color.yellow;
	//    if (HP <= (initialHP / 4)) GUI.color = Color.red;
	//    GUI.DrawTexture(new Rect(pos.x - 25, Screen.height - pos.y + 21, (float)HP / initialHP * (float)50f, 5), Box);
	//}
}