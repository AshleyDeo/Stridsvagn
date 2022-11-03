using UnityEngine;

public class PlayerTank : TankBase, IPickup, ISaveable {
	[SerializeField] private AmmoType originalAmmo;
	private int ammoCountdown = 0;

	bool _controllable = true;
	private float lts, rts, tlts, trts;
	bool shooting = false;
	protected override void Awake() {
		base.Awake();
		_tank = GameManager.Instance._tank;
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
		Ammo = originalAmmo;
	}
	protected override void Update() {
		if (!_controllable) return;
		base.Update();
		float spread = Random.Range(-0.5f + (float)Health.HP / (_tank.maxHealth * 2f), 0.5f - (float)Health.HP / (_tank.maxHealth * 2f));
		firePoint.localRotation = Quaternion.Euler(0, 0, spread * 1.25f);
		if (ammoCountdown <= 0) {
			Ammo = originalAmmo;
		}

		lts = Input.GetAxis("Vertical");
		rts = Input.GetAxis("Vertical2");
		tlts = Input.GetAxis("RotateTurretL");
		trts = Input.GetAxis("RotateTurretR");
		shooting = Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2");

		if (firePoint != null) {
			if (shooting && nextFire <= 0f) {
				nextFire = _tank.fireRate;
				if (ammoCountdown > 0) ammoCountdown--;
				Shoot();
			}
		}
		//VCamController.Instance.ShakeCamera(0.5f, 0.5f);
	}
	void FixedUpdate() {
		if (!_controllable) return;
		//TANK
		//Forward movement if both triggers depressed
		if (rts > 0f && lts > 0f) { Move(_tank.moveSpeed); }
		//Backward movement if both triggers depressed
		if (rts < 0f && lts < 0f) { Move(-_tank.moveSpeed); }
		//Track left
		if ((lts > 0f) || (rts < 0f) || (lts > 0f && rts < 0f)) {
			Rotate(-_tank.turnSpeed);
		}
		//Track right
		if ((lts < 0f) || (rts > 0f) || (rts > 0f && lts < 0f)) {
			Rotate(_tank.turnSpeed);
		}

		//TURRET
		if (tlts > 0f) { RotateTurret(_tank.turretRotSpeed); } //Rotate left
		if (trts > 0f) { RotateTurret(-_tank.turretRotSpeed); } //Rotate right

	}
	public override void Damage(int damage) {
		base.Damage(damage);
		VCamController.Instance.ShakeCamera(0.5f, 0.5f);
	}
	public void AddAmmo(AmmoType ammoT, int amount) {
		Ammo = ammoT;
		ammoCountdown = amount;
	}
	protected override void Shoot() {
		base.Shoot();
		if (!Ammo.isDrop) {
			VCamController.Instance.ShakeCamera(0.1f, 0.3f);
		}
	}
	protected override void OnDead() {
		base.OnDead();
		VCamController.Instance.ShakeCamera(1f, 1f);
		_controllable = false;
		body.gameObject.SetActive(false);
		turret.gameObject.SetActive(false);
	}
	public object SaveState() {
		return new SaveData() {
			tank = this._tank,
			health = this.Health,
			position = this.transform.position,
			origAmmo = this.originalAmmo,
			ammo = this.Ammo,
			ammoLeft = this.ammoCountdown
		};
	}
	public void LoadState(object state) {
		var saveData = (SaveData)state;
		_tank = saveData.tank;
		Health = saveData.health;
		this.transform.position = saveData.position;
		originalAmmo = saveData.ammo;
		Ammo = saveData.ammo;
		ammoCountdown = saveData.ammoLeft;
	}
	[System.Serializable]
	private struct SaveData {
		public TankType tank;
		public Health health;
		public Vector3 position;
		public AmmoType origAmmo;
		public AmmoType ammo;
		public int ammoLeft;
	}
}