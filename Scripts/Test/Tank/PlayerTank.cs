using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTank : TankBase, IPickup, ISaveable {
	PlayerControls _input;
	private int ammoCountdown = 0;

	private float lts = 0f, rts = 0f, trts = 0f;
	bool shooting = false;
	//Events
	public static event Action<bool> OnPlayerDead;
	protected override void Awake() {
		if (_tank == null) { _tank = GameManager.Instance._tank; }
		base.Awake();
	}
	void OnEnable() {
		_input = new PlayerControls();
		_input.Player.Enable();
		_input.Player.TankLeftTrack.performed += OnTankLeftInput;
		_input.Player.TankRightTrack.performed += OnTankRightInput;
		_input.Player.TurretRotate.performed += OnTurretInput;
		_input.Player.Shoot.performed += OnShootInput;
		_input.Player.TankLeftTrack.canceled += OnTankLeftInput;
		_input.Player.TankRightTrack.canceled += OnTankRightInput;
		_input.Player.TurretRotate.canceled += OnTurretInput;
		_input.Player.Shoot.canceled += OnShootInput;
	}
	void OnDisable() {
		_input.Player.TankLeftTrack.performed -= OnTankLeftInput;
		_input.Player.TankRightTrack.performed -= OnTankRightInput;
		_input.Player.TurretRotate.performed -= OnTurretInput;
		_input.Player.Shoot.performed -= OnShootInput;
		_input.Player.TankLeftTrack.canceled -= OnTankLeftInput;
		_input.Player.TankRightTrack.canceled -= OnTankRightInput;
		_input.Player.TurretRotate.canceled -= OnTurretInput;
		_input.Player.Shoot.canceled -= OnShootInput;
		_input.Disable();
	}
	void Start() {
		_controllable = loaded;
		if (spawnPoint == null) return;
		transform.position = spawnPoint.position;
	}
	protected override void Update() {
		base.Update();
		if (ammoCountdown <= 0) {
			Ammo = _tank.originalAmmo;
		}
	}
	void FixedUpdate() {
		if (!_controllable) return;
		//TANK
		//Forward movement if both triggers depressed
		if (rts * lts != 0f && rts != lts) { Move(rts); }
		else {
			//Backward movement if both triggers depressed
			//Track left
			if ((lts > 0f) || (rts < 0f)) {
				Rotate((lts + rts) / 2);
			}
			//Track right
			if ((lts < 0f) || (rts > 0f)) {
				Rotate((lts + rts) / 2);
			}
		}
		//TURRET
		RotateTurret(trts);

		if (firePoint != null) {
			if (shooting && nextFire <= 0f) {
				nextFire = _tank.fireRate;
				if (ammoCountdown > 0) ammoCountdown--;
				Shoot();
			}
		}
	}
	public void SetControlleble(bool can) => _controllable = can;
	private void OnTankLeftInput(InputAction.CallbackContext ctx) {
		lts = ctx.ReadValue<float>();
	}
	private void OnTankRightInput(InputAction.CallbackContext ctx) {
		rts = ctx.ReadValue<float>();
	}
	private void OnTurretInput(InputAction.CallbackContext ctx) {
		trts = ctx.ReadValue<float>();
	}
	private void OnShootInput(InputAction.CallbackContext ctx) {
		shooting = ctx.ReadValue<float>() == 1f;
	}
	public override void Damage(int damage) {
		base.Damage(damage);
		VCamController.Instance.ShakeCamera(0.5f, 0.5f);
	}
	public void AddAmmo(AmmoType ammoT, int amount) {
		if (Ammo == ammoT) {
			ammoCountdown += amount;
			return;
		}
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
		OnPlayerDead(true);
	}
	public object SaveState() {
		return new SaveData() {
			tank = this._tank,
			health = this.Health,
			position = this.transform.position,
			ammo = this.Ammo,
			ammoLeft = this.ammoCountdown
		};
	}
	public void LoadState(object state) {
		var saveData = (SaveData)state;
		_tank = saveData.tank;
		Health = saveData.health;
		this.transform.position = saveData.position;
		Ammo = saveData.ammo;
		ammoCountdown = saveData.ammoLeft;
	}
	[System.Serializable]
	private struct SaveData {
		public TankType tank;
		public Health health;
		public Vector3 position;
		public AmmoType ammo;
		public int ammoLeft;
	}
}