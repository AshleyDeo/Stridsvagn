using UnityEngine;
using UnityEngine.UIElements;

public class EnemyTank : TankBase {
	bool canFire;
	private RaycastHit2D bogey;

	private Transform player;
	private float distanceToPlayer;
	Vector3 sensor_front;
	/*
	 ACTIONS
	 * patrol 
	 * chase rad = detect dist * 1.5
	 * aim rad = detectdist
	 */
	protected void Start() {
		player = GameControllerTest.Instance.player;
		sensor_front = transform.position + (transform.up * 5f);
		sensor_front.z = -0.5f;
	}
	protected override void Update() {
		base.Update();
		//distanceToPlayer = Vector2.Distance(player.position, transform.position); 
		//if (nextFire > 0f && distanceToPlayer <= _tank.detectDist) {
		//	Shoot();
		//}
		if (firePoint != null) {
			if (canFire && nextFire <= 0f) {
				nextFire = _tank.fireRate;
				Shoot();
			}
		}
	}
	private void FixedUpdate() {
		Sensors();
		//Detect Player
		RaycastHit2D[] detection = Physics2D.CircleCastAll(transform.position, _tank.detectDist, transform.forward, 0.0f, 1 << 6);
		float closestDistanceSqr = Mathf.Infinity;

		foreach (RaycastHit2D blip in detection) {
			Vector2 directionToTarget = blip.transform.position - transform.position;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if (dSqrToTarget < closestDistanceSqr) {
				closestDistanceSqr = dSqrToTarget;
				bogey = blip;
			}
		}
		if (bogey) {
			float angle = AngleBetweenPoints(turret.position, bogey.transform.position);
			Vector2 directionToBogey = bogey.transform.position - transform.position;
			float dSqrToBogey = directionToBogey.sqrMagnitude;
			if (dSqrToBogey <= Mathf.Pow(_tank.detectDist, 2)) {
				rb.rotation = angle + 90f;
			}
		}
		else {
			turret.localRotation = Quaternion.Euler(0, 0, 0);
		}
		RaycastHit2D sight = Physics2D.Raycast(firePoint.position, turret.up, _tank.detectDist - 1.0f, 9 << 6);
		canFire = (sight.collider != null) && sight.collider.CompareTag("Player");
	}
	private float AngleBetweenPoints(Vector2 a, Vector2 b) => Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	protected override void Shoot() {
		if (!canFire) return;
		base.Shoot();
	}
	protected override void OnDead() {
		base.OnDead();
		Destroy(gameObject);
	}
	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Wall")) {
			Turn180();
		}
	}
	void Sensors() {
		RaycastHit2D guard = Physics2D.BoxCast(sensor_front, new(0.67f, 0.67f), 0.0f, transform.forward, 0.0f, 13 << 6);
	}
	void OnDrawGizmos() {
		Gizmos.color = Color.gray;
		Gizmos.DrawCube(sensor_front, new(0.67f, 0.67f, 0.2f));
	}
	protected void Turn180() {
		float newRot = transform.rotation.z + 180;
		while (Mathf.Abs(transform.rotation.z - newRot) < 0.1f) {
			Rotate(1f);
		}
	}
	void RotateTo(float angle) {
		while(Mathf.Abs(turret.rotation.z - angle) > 0.1f) {
			RotateTurret(1f);
		}
	}
}