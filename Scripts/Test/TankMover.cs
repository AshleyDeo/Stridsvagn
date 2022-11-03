using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TankMover : MonoBehaviour {
	TankCreator creator;
	[SerializeField] private Transform turret;
	[SerializeField] private Transform firePoint;
	[SerializeField] private GameObject muzzleFlash;
	[SerializeField] private bool canRotate;

	[SerializeField] private GameObject shield;
	private Coroutine shieldOn = null;
	public AmmoType ammo { get; private set; }
	void Awake() {
		creator = GetComponent<TankCreator>();
	}
	void Start() {
		firePoint = creator.firePoint;
		firePoint.parent = turret;
	}
	void Update() {
		float spread = Random.Range(-0.5f + (float)creator.Health.HP / (creator.GetTank.maxHealth * 2f), 0.5f - (float)creator.Health.HP / (creator.GetTank.maxHealth * 2f));
		firePoint.localRotation = Quaternion.Euler(0, 0, spread * 1.25f);
	}
	public void SetRoate(bool rotate) { canRotate = rotate; }
	public void SetAmmo(AmmoType ammoT) { ammo = ammoT; }
	public void MoveForward(float speed) {
		transform.Translate(speed * Time.deltaTime * Vector3.up);
	}
	public void MovBackward(float speed) {
		transform.Translate(-speed * Time.deltaTime * Vector3.up);
	}
	public void RotateClock(float speed) {
		transform.Rotate(Vector3.forward, speed * Time.deltaTime);
	}
	public void RotateCounterClock(float speed) {
		transform.Rotate(Vector3.forward, -speed * Time.deltaTime);
	}
	public void RotateTurretClock(float speed) {
		if (!canRotate) return;
		turret.Rotate(Vector3.forward, -speed * Time.deltaTime);
	}
	public void RotateTurretCounterClock(float speed) {
		if (!canRotate) return;
		turret.Rotate(Vector3.forward, speed * Time.deltaTime);
	}
	public void Shoot() {
		if (ammo.isDrop) { ammo.Use(this.transform); }
		else {
			ammo.Use(firePoint);
			var muzzleVfx = Instantiate(muzzleFlash, firePoint.position, firePoint.rotation) as GameObject;
			muzzleVfx.transform.parent = creator.turret;
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
	public void DeactivateShield() {
		if (shieldOn != null) { 
			StopCoroutine(shieldOn);
			shieldOn = null;
		}
		shield.SetActive(false);
	}
}