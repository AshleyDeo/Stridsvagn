using UnityEngine;

public class EnemyTest : TankBase {
	bool canFire;
	private float spread;
	private RaycastHit2D bogey;

	private Transform player;
	private float distanceToPlayer;
	/*
	 ACTIONS
	 * patrol 
	 * chase rad = detect dist * 1.5
	 * aim rad = detectdist
	 
	 */
	protected override void Awake() {
		base.Awake();
		Health = GetComponent<Health>();
		Health.SetHP(_tank.maxHealth);
		GameObject bodyObj = Instantiate(_tank.Hull, body.position + _tank.bodyPosition, _tank.bodyAngle) as GameObject;
		GameObject turretObj = Instantiate(_tank.Turret, turret.position + _tank.turretPosition, _tank.turretAngle) as GameObject;
		bodyObj.transform.parent = body;
		turretObj.transform.parent = turret;
		bodyObj.transform.tag = turretObj.transform.tag = "Player";
		Transform point = turretObj.GetComponent<TankInfo>().firePoint;
		if (point == null) {
			point = bodyObj.GetComponent<TankInfo>().firePoint;
		}
		firePoint = point;
		firePoint.parent = turret;
		canRotate = _tank.Turret.GetComponent<TankInfo>().canRotate;
	}
	protected void Start() {
		player = GameControllerTest.Instance.player;
	}
	protected override void Update() {
		base.Update();
		distanceToPlayer = Vector2.Distance(player.position, transform.position); 
		if (nextFire > 0f && distanceToPlayer <= _tank.detectDist) {
			Shoot();
		}
	}
	private void FixedUpdate() {
		Move(_tank.moveSpeed);

		spread = Random.Range(-GameController.enemyCount - 3.0f, GameController.enemyCount + 3.0f);
		firePoint.localRotation = Quaternion.Euler(0, 0, spread);

		RaycastHit2D[] detection = Physics2D.CircleCastAll(transform.position, _tank.detectDist, transform.up, 0.0f, 1 << 6);
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
			float angle = AngleBetweenPoints(rb.position, bogey.transform.position);
			Vector2 directionToBogey = bogey.transform.position - transform.position;
			float dSqrToBogey = directionToBogey.sqrMagnitude;
			if (dSqrToBogey <= Mathf.Pow(_tank.detectDist, 2)) rb.rotation = angle + 90f;
		}
		else transform.localRotation = Quaternion.Euler(0, 0, 0);

		RaycastHit2D sight = Physics2D.Raycast(firePoint.position, transform.up, _tank.detectDist - 1.0f, 9 << 6);
		if (sight.collider != null && sight.collider.CompareTag("Tank")) canFire = true;
		else canFire = false;
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
			transform.localRotation *= Quaternion.Euler(0, 0, 180);
		}
	}
}