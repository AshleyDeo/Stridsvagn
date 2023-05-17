using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerTank : MonoBehaviour, IPickup, IDestructible {
	private Rigidbody _rb;
	
	[SerializeField] private TankType _tank = null;
	private Health _health = null;
	//public Stats Stats { get; private set; }

	[SerializeField] private Transform _spawnPoint = null;
	[SerializeField] private Transform _body, _turret;
	[SerializeField] private Transform _firePoint = null;
	[SerializeField] private GameObject _muzzleFlash;
	[SerializeField] private GameObject _explosion;

	[SerializeField] private GameObject _shield;
	private Coroutine _shieldOn = null;

	private bool _isDead = false;

	private PlayerInput _input;
	private float _lts = 0f, _rts = 0f, _trts = 0f;
	[SerializeField] private AmmoType _ammo;
	private int _ammoCountdown = 0;
	private float _nextFire = 0f;

	[SerializeField] private bool _canRotate;

	[Header("Debug", order = 3)]
	private bool _loaded = false;
	private bool _controllable = true;

	[SerializeField] private Logger _logger;
	[SerializeField] private bool _showLogs;
	[SerializeField] private bool _showGizmos;
	//Events
	public static event Action<bool> OnPlayerDead;

	private void Awake() {
		if (_tank == null) { _tank = GameManager.Instance.Tank; }
		_rb = GetComponent<Rigidbody>();
		_health = GetComponent<Health>();
		_input = GetComponent<PlayerInput>();
		_loaded = LoadTank();
		if (!_loaded) {
			_health.SetHP(10);
		}
	}
	private void Start() {
		_controllable = _loaded;
		if (_spawnPoint == null) return;
		transform.position = _spawnPoint.position;
	}
	private void Update() {
		_rb.velocity = Vector3.zero;
		if (_nextFire > 0f) { _nextFire -= Time.deltaTime; }
		float spread = UnityEngine.Random.Range(-0.5f + (float)_health.HP / (_tank.MaxHealth * 2f), 0.5f - (float)_health.HP / (_tank.MaxHealth * 2f));
		_firePoint.localRotation = Quaternion.Euler(0, spread * 1.25f, 0);
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
	#region Input
	public void SetControlleble(bool can) => _controllable = can;
	private void Move (float speed) {
		Vector3 dir = speed * _tank.MoveSpeed * Time.fixedDeltaTime * transform.forward;
		_rb.MovePosition(transform.position + dir);
	}
	private void Rotate (float speed) {
		transform.Rotate(Vector3.up, speed * _tank.TurnSpeed * Time.deltaTime);
	}
	private void RotateTurret (float speed) {
		if (!_canRotate) return;
		_turret.Rotate(Vector3.up, speed * _tank.TurretRotSpeed * Time.deltaTime);
	}
	#endregion
	public void Damage(int damage) {
		_health.DecreaseHP(damage);
		if (_health.HP <= 0 && !_isDead) {
			OnDead();
			_isDead = true;
		}
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
	private void Shoot() {
		if (_firePoint == null || _ammo == null) {
			Log("Tank - Shoot(): No firePoint");
			return;
		}
		if (_ammo.IsDrop) { _ammo.Use(transform); }
		else {
			_ammo.Use(_firePoint);
			var muzzleVfx = Instantiate(_muzzleFlash, _firePoint.position, _firePoint.rotation) as GameObject;
			muzzleVfx.transform.parent = _turret;
			VCamController.Instance.ShakeCamera(0.1f, 0.3f);
			Destroy(muzzleVfx, 5);
		}
	}
	private void OnDead() {
		GameObject exp = Instantiate(_explosion, transform, false) as GameObject;
		exp.transform.parent = null;
		Instantiate(_tank.Husk, transform.position, transform.rotation);
		VCamController.Instance.ShakeCamera(1f, 1f);
		OnPlayerDead?.Invoke(true);
		Destroy(exp, 5);
	}
	#region Shield
	public void ActivateShield (float time) {
		_shield.SetActive(true);
		_shieldOn = StartCoroutine(ShieldTimer(time));
	}
	IEnumerator ShieldTimer (float time) {
		yield return new WaitForSeconds(time);
		_shieldOn = null;
		DeactivateShield();
	}
	private void DeactivateShield () {
		if (_shieldOn != null) {
			StopCoroutine(_shieldOn);
			_shieldOn = null;
		}
		_shield.SetActive(false);
	}
	#endregion
	public AmmoType GetProjectile () => _ammo;
	public void SetTank (TankType tank) {
		_tank = tank;
		LoadTank();
	}
	protected void ResetTank () {
		foreach (Transform child in _body) {
			Destroy(child.gameObject);
		}
		foreach (Transform child in _turret) {
			Destroy(child.gameObject);
		}
		_body.position = _turret.position = transform.position;
		transform.rotation = _body.rotation = _turret.rotation = Quaternion.identity;
	}
	protected bool LoadTank () {
		if (_tank == null) return false;
		ResetTank();
		if (_health == null) _health = GetComponent<Health>();
		_health.SetHP(_tank.MaxHealth);
		_ammo = _tank.OriginalAmmo;
		Transform bodyObj = Instantiate(_tank.Hull, _body.position + _tank.BodyPosition, _tank.BodyAngle, _body).transform;
		Transform turretObj = Instantiate(_tank.Turret, _turret.position + _tank.TurretPosition, _tank.TurretAngle, _turret).transform;

		bodyObj.tag = turretObj.tag = _body.tag;

		//Move fire point to child of turret cuz physics
		if (bodyObj.GetComponent<TankInfo>().firePoint) {
			_firePoint = bodyObj.GetChild(0);
			_firePoint.parent = _body;
		}
		else {
			_firePoint = turretObj.GetChild(0);
			_firePoint.parent = _turret;
		}
		_firePoint.rotation = Quaternion.identity;
		_canRotate = _tank.Turret.GetComponent<TankInfo>().canRotate;

		//Move collider from tank prefab to player object
		BoxCollider collider = GetComponent<BoxCollider>();
		BoxCollider childCol = bodyObj.GetChild(0).GetComponent<BoxCollider>();
		collider.size = childCol.size;
		collider.center = childCol.center;
		transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
		Destroy(childCol);
		return true;
	}

	#region Debug
	protected void Log (string message) {
		if (!_showLogs) return;
		if (_logger) _logger.Log(message, this);
		else Debug.Log(message);
	}
	protected virtual void OnDrawGizmos () {
		if (_showGizmos) {
			//Detection Range
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, _tank.DetectDist);
		}
	}
	#endregion
}