using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class TankBaseNetwork : NetworkBehaviour {
	[SerializeField] protected TankType _tank = null;
	protected Health Health;

	protected Rigidbody2D rb;
	[SerializeField] protected Transform body, turret;
	[SerializeField] protected Transform firePoint;
	protected float nextFire = 0f;
	[SerializeField] private GameObject muzzleFlash;
	[SerializeField] protected bool canRotate;

	[SerializeField] private GameObject shield;
	private Coroutine shieldOn = null;
	//public Stats Stats { get; protected set; }
	[SerializeField] protected AmmoType Ammo;

	[SerializeField] private GameObject explosion;
	protected bool isDead = false;

	protected virtual void Awake() {
		rb = GetComponent<Rigidbody2D>();
	}
	protected virtual void Update() {
		rb.velocity = Vector2.zero;
		if (nextFire > 0f) { nextFire -= Time.deltaTime; }
	}
	public AmmoType GetProjectile() => Ammo;
	public virtual void Damage(int damage) {
		Debug.Log("IDestructible: Damage");
		//VCamController.Instance.ShakeCamera(0.5f, 0.5f);
		Health.DecreaseHP(damage);
		if (Health.HP <= 0 && !isDead) {
			OnDead();
			isDead = true;
		}
	}
	protected virtual void OnDead() {
		GameObject exp = Instantiate(explosion, transform, false) as GameObject;
		exp.transform.parent = null;
		GameObject remains = Instantiate(_tank.Husk, transform.position, transform.rotation) as GameObject;
		remains.transform.parent = null;
		Destroy(exp, 5);
	}
	protected void Move(float speed) {
		Vector3 dir = speed *  Time.fixedDeltaTime * transform.up;
		rb.MovePosition(transform.position + dir);
	}
	protected void Rotate(float speed) {
		transform.Rotate(Vector3.forward, speed * Time.deltaTime);
	}
	protected void RotateTurret(float speed) {
		if (!canRotate) return;
		turret.Rotate(Vector3.forward, speed * Time.deltaTime);
	}
	protected virtual void Shoot() {
		if (Ammo.isDrop) { Ammo.Use(this.transform); }
		else {
			Ammo.Use(firePoint);
			var muzzleVfx = Instantiate(muzzleFlash, firePoint.position, firePoint.rotation) as GameObject;
			muzzleVfx.transform.parent = turret;
			Destroy(muzzleVfx, 5);
		}
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