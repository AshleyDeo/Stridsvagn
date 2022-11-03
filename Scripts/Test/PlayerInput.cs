using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour, IPickup{
	[SerializeField] private TankType tank = null;
	TankCreator creator;
	TankMover mover;
	[SerializeField] private Transform firePoint, spawnPoint;
	[SerializeField] private AmmoType originalAmmo;
	private float lts, rts, tlts, trts;
	private float nextFire = 0f;
	bool  _controllable = false;
	bool shooting = false;
	private Vector3 destination;
	private int ammoCountdown = 0;
	void Awake() {
		Debug.Log("Player Input Awake");
		if (tank == null) { tank = GameManager.Instance._tank; }
		creator = GetComponent<TankCreator>();
		mover = GetComponent<TankMover>();
		creator.CreateTank(tank);
		firePoint = creator.firePoint;
	}
	void Update() {
		if (ammoCountdown <= 0) { 
			mover.SetAmmo(originalAmmo); 
		}
		if (nextFire > 0f) { nextFire -= Time.deltaTime; }
		if (creator.Health.HP < 0f) { OnDead(); }

		lts = Input.GetAxis("Vertical");
		rts = Input.GetAxis("Vertical2");
		tlts = Input.GetAxis("RotateTurretL");
		trts = Input.GetAxis("RotateTurretR");
		shooting = Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2");

		//TANK
		//Forward movement if both triggers depressed
		if (rts > 0f && lts > 0f) { mover.MoveForward(tank.moveSpeed); }
		//Backward movement if both triggers depressed
		if (rts < 0f && lts < 0f) { mover.MovBackward(tank.moveSpeed); }
		//Track left
		if ((lts > 0f) || (rts < 0f) || (lts > 0f && rts < 0f)) {
			mover.RotateCounterClock(tank.turnSpeed);
		}
		//Track right
		if ((lts < 0f) || (rts > 0f) || (rts > 0f && lts < 0f)) {
			mover.RotateClock(tank.turnSpeed);
		}

		//TURRET
		if (tlts > 0f) { mover.RotateTurretCounterClock(tank.turretRotSpeed); } //Rotate left
		if (trts > 0f) { mover.RotateTurretClock(tank.turretRotSpeed); } //Rotate right

		if (firePoint != null) {
			if (shooting && nextFire <= 0f) {
				nextFire = tank.fireRate;
				if (ammoCountdown > 0) ammoCountdown--;
				mover.Shoot();
			}

			Debug.DrawRay(firePoint.position, firePoint.up, Color.green);

			RaycastHit2D sight = Physics2D.Raycast(firePoint.position, firePoint.up, 7.0f, 3 << 8);
		}
	}
	public string GetProjectile() => mover.ammo.ammoName;
	public void AddAmmo(AmmoType ammoT, int amount) {
		mover.SetAmmo(ammoT);
		ammoCountdown = amount;
	}
	public void ActivateShield(float time) { mover.ActivateShield(time); }
	void OnDead() {
		_controllable = false;
		VCamController.Instance.ShakeCamera(2f, 1f);
		int lives = GameManager.Instance._currLives;

		if (lives > 0) {
			Debug.Log("You have " + lives + " lives left");
			GameManager.Instance._currLives--;
			PlayerPrefs.SetInt("livesLeft", lives);
			gameObject.transform.localScale = new Vector3(0, 0, 0);
			StartCoroutine(Respawn());
		}
		else {
			Debug.Log("No lives left");
			GameController.allyCount--;
			gameObject.SetActive(false);
			//GetComponent<AudioSource>().Stop();
		}
	}
	IEnumerator Respawn() {
		yield return new WaitForSeconds(2.0f);
		transform.position = spawnPoint.position;
		creator.isDead = false;
		creator.Health.ResetHP();
		_controllable = true;
	}
}