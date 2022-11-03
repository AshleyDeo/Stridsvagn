using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TankBase : MonoBehaviour, IDestructible {
	[SerializeField] protected TankType _tank = null;
	protected Health Health = null;
	//public Stats Stats { get; protected set; }

	protected Rigidbody2D rb;
	[SerializeField] protected Transform spawnPoint = null;
	[SerializeField] protected Transform body, turret;
	[SerializeField] protected Transform firePoint = null;
	[SerializeField] private GameObject muzzleFlash;
	protected float nextFire = 0f;
	[SerializeField] protected bool canRotate;

	[SerializeField] private GameObject shield;
	private Coroutine shieldOn = null;
	[SerializeField] protected AmmoType Ammo;

	[SerializeField] private GameObject explosion; 
	protected bool isDead = false;
	protected GameObject remains = null;

	[SerializeField] protected bool UiVer = false;
	protected bool loaded = false;
	protected bool _controllable = true;

	protected virtual void Awake() {
		rb = GetComponent<Rigidbody2D>();
		Health = GetComponent<Health>();
		loaded = LoadTank();
		if (!loaded) {
			Health.SetHP(10);
		}
	}
	protected virtual void Update() {
		rb.velocity = Vector2.zero;
		if (nextFire > 0f) { nextFire -= Time.deltaTime; }
		float spread = Random.Range(-0.5f + (float)Health.HP / (_tank.maxHealth * 2f), 0.5f - (float)Health.HP / (_tank.maxHealth * 2f));
		firePoint.localRotation = Quaternion.Euler(0, 0, spread * 1.25f);
	}
	protected void Move(float speed) {
		Vector3 dir = speed * _tank.moveSpeed *  Time.fixedDeltaTime * transform.up;
		rb.MovePosition(transform.position + dir);
	}
	protected void Rotate(float speed) {
		transform.Rotate(Vector3.forward, speed * _tank.turnSpeed * Time.deltaTime);
	}
	protected void RotateTurret(float speed) {
		if (!canRotate) return;
		turret.Rotate(Vector3.forward, speed * _tank.turretRotSpeed * Time.deltaTime);
	}
	public AmmoType GetProjectile() => Ammo;
	protected virtual void Shoot() {
		if (firePoint == null || Ammo == null) {
			Debug.Log("TankBase - Shoot(): No firePoint");
			return;
		}
		if (Ammo.isDrop) { Ammo.Use(this.transform); }
		else {
			Ammo.Use(firePoint);
			var muzzleVfx = Instantiate(muzzleFlash, firePoint.position, firePoint.rotation) as GameObject;
			muzzleVfx.transform.parent = turret;
			Destroy(muzzleVfx, 5);
		}
	}
	public virtual void Damage(int damage) {
		Health.DecreaseHP(damage);
		if (Health.HP <= 0 && !isDead) {
			OnDead();
			isDead = true;
		}
	}
	protected virtual void OnDead() {
		GameObject exp = Instantiate(explosion, transform, false) as GameObject;
		exp.transform.parent = null;
		remains = Instantiate(_tank.Husk, transform.position, transform.rotation) as GameObject;
		Destroy(exp, 5);
	}
	protected virtual void Respawn() {
		if (spawnPoint != null) { transform.position = spawnPoint.position; }
		loaded = LoadTank();
		if(!loaded) return;
		Destroy(remains);
		remains = null;
		isDead = false;
	}
	public void SetTank(TankType tank) {
		_tank = tank;
		LoadTank();
	}
	protected void ResetTank() {
		foreach (Transform child in body) {
			Destroy(child.gameObject);
		}
		foreach (Transform child in turret) {
			Destroy(child.gameObject);
		}
		body.position = turret.position = transform.position;
		transform.rotation = body.rotation = turret.rotation = Quaternion.identity;
	}
	protected bool LoadTank() {
		if (_tank == null) return false;
		ResetTank();
		if (Health == null) Health = GetComponent<Health>();
		Health.SetHP(_tank.maxHealth);
		Ammo = _tank.originalAmmo;
		GameObject bodyObj;
		GameObject turretObj;
		if (UiVer) {
			bodyObj = Instantiate(_tank.UI_Hull, body.position + _tank.bodyPosition, _tank.bodyAngle) as GameObject;
			turretObj = Instantiate(_tank.UI_Turret, turret.position + _tank.turretPosition, _tank.turretAngle) as GameObject;
		}
		else {
			bodyObj = Instantiate(_tank.Hull, body.position + _tank.bodyPosition, _tank.bodyAngle) as GameObject;
			turretObj = Instantiate(_tank.Turret, turret.position + _tank.turretPosition, _tank.turretAngle) as GameObject;
		}
		bodyObj.transform.parent = body;
		turretObj.transform.parent = turret;
		bodyObj.transform.tag = turretObj.transform.tag = body.tag;
		if (bodyObj.GetComponent<TankInfo>().firePoint) {
			firePoint = bodyObj.transform.GetChild(0);
			firePoint.parent = body;
		}
		if (turretObj.GetComponent<TankInfo>().firePoint) {
			firePoint = turretObj.transform.GetChild(0);
			firePoint.parent = turret;
		}
		firePoint.rotation = Quaternion.identity;
		canRotate = _tank.Turret.GetComponent<TankInfo>().canRotate;
		return true;
	}
	public void ActivateShield(float time) {
		shield.SetActive(true);
		shieldOn = StartCoroutine(ShieldTimer(time));
	}
	IEnumerator ShieldTimer(float time) {
		yield return new WaitForSeconds(time);
		shieldOn = null;
		DeactivateShield();
	}
	protected void DeactivateShield() {
		if (shieldOn != null) {
			StopCoroutine(shieldOn);
			shieldOn = null;
		}
		shield.SetActive(false);
	}
}