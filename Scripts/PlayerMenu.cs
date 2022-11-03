using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
public class PlayerMenu : TankChoose, IPickup {
	PlayerControls _input;
	private int ammoCountdown = 0;

	private float lts = 0f, rts = 0f, trts = 0f;
	bool shooting = false;
	protected override void Awake() {
		base.Awake();
		_controllable = false;
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
	}
}