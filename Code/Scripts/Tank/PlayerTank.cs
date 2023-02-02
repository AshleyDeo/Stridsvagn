using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTank : TankBase, IPickup, ISaveable {
	[Header("Player")]
	private PlayerInput _input;
	private float _lts = 0f, _rts = 0f, _trts = 0f;
	private int _ammoCountdown = 0;
	//Events
	public static event Action<bool> OnPlayerDead;
	protected override void Awake() {
		if (_tank == null) { _tank = GameManager.Instance.Tank; }
		_input = GetComponent<PlayerInput>();
		base.Awake();
	}
	protected override void Start() {
		_controllable = _loaded;
		base.Start();
	}
	protected override void Update() {
		base.Update();
		if (_ammoCountdown <= 0) {
			_ammo = _tank.OriginalAmmo;
		}
		_lts = _input.actions["Move Left Track"].ReadValue<float>();
		_rts = _input.actions["Move Right Track"].ReadValue<float>();
		_trts = _input.actions["Rotate Turret"].ReadValue<float>();

		if (_firePoint != null) {
			if (_input.actions["Shoot"].triggered && _nextFire <= 0f) {
				_nextFire = _tank.FireRate;
				if (_ammoCountdown > 0) _ammoCountdown--;
				Shoot();
			}
		}
	}
	void FixedUpdate() {
		if (!_controllable) return;
		//TANK
		//Forward movement if both triggers depressed
		if (_rts * _lts != 0f && _rts != _lts) { Move(-_rts); }
		else {
			//Backward movement if both triggers depressed
			//Track left
			if ((_lts > 0f) || (_rts < 0f)) {
				Rotate((_lts + _rts) / 2);
			}
			//Track right
			if ((_lts < 0f) || (_rts > 0f)) {
				Rotate((_lts + _rts) / 2);
			}
		}
		//TURRET
		RotateTurret(_trts);
	}
	public void SetControlleble(bool can) => _controllable = can;
	public override void Damage(int damage) {
		base.Damage(damage);
		VCamController.Instance.ShakeCamera(0.5f, 0.5f);
	}
	public void AddAmmo(AmmoType ammoT, int amount) {
		if (_ammo == ammoT) {
			_ammoCountdown += amount;
			return;
		}
		_ammo = ammoT;
		_ammoCountdown = amount;
	}
	protected override void Shoot() {
		base.Shoot();
		if (!_ammo.IsDrop) {
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
			Tank = this._tank,
			Health = this._health,
			Position = this.transform.position,
			Ammo = this._ammo,
			AmmoLeft = this._ammoCountdown
		};
	}
	public void LoadState(object state) {
		var saveData = (SaveData)state;
		_tank = saveData.Tank;
		_health = saveData.Health;
		this.transform.position = saveData.Position;
		_ammo = saveData.Ammo;
		_ammoCountdown = saveData.AmmoLeft;
	}
	[System.Serializable]
	private struct SaveData {
		public TankType Tank;
		public Health Health;
		public Vector3 Position;
		public AmmoType Ammo;
		public int AmmoLeft;
	}
}