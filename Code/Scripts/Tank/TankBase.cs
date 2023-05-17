using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankBase : MonoBehaviour, IDestructible {
	[Header("Tank Base", order = 0)]
	[SerializeField] protected TankType _tank = null;
	protected Health _health = null;
	//public Stats Stats { get; protected set; }

	protected Rigidbody _rb;
	[SerializeField] protected Transform _spawnPoint = null;
	[SerializeField] protected Transform _body, _turret;
	[SerializeField] protected Transform _firePoint = null;
	[SerializeField] private GameObject _muzzleFlash;
	protected float _nextFire = 0f;

	[SerializeField] private GameObject _shield;
	private Coroutine _shieldOn = null;
	[SerializeField] protected AmmoType _ammo;

	[SerializeField] private GameObject _explosion; 
	protected bool _isDead = false;
	protected GameObject _remains = null;

	[SerializeField] protected bool _canRotate;
	[SerializeField] protected bool _uiVer = false;
	protected bool _loaded = false;
	protected bool _controllable = true;

	[Header("Debug", order = 3)]
	[SerializeField] protected Logger _logger;
	[SerializeField] protected bool _showLogs;
	[SerializeField] protected bool _showGizmos;

	protected virtual void Awake() {
		_rb = GetComponent<Rigidbody>();
		_health = GetComponent<Health>();
		_loaded = LoadTank();
		if (!_loaded) {
			_health.SetHP(10);
		}
	}
	protected virtual void Start() {
		if (_spawnPoint == null) return;
		transform.position = _spawnPoint.position;
	}
	protected virtual void Update() {
		_rb.velocity = Vector3.zero;
		if (_nextFire > 0f) { _nextFire -= Time.deltaTime; }
		float spread = Random.Range(-0.5f + (float)_health.HP / (_tank.MaxHealth * 2f), 0.5f - (float)_health.HP / (_tank.MaxHealth * 2f));
		_firePoint.localRotation = Quaternion.Euler(0, spread * 1.25f, 0);
	}
	protected void Move(float speed) {
		Vector3 dir = speed * _tank.MoveSpeed *  Time.fixedDeltaTime * transform.forward;
		_rb.MovePosition(transform.position + dir);
	}
	protected void Rotate(float speed) {
		transform.Rotate(Vector3.up, speed * _tank.TurnSpeed * Time.deltaTime);
	}
	protected void RotateTurret(float speed) {
		if (!_canRotate) return;
		_turret.Rotate(Vector3.up, speed * _tank.TurretRotSpeed * Time.deltaTime);
	}
	public AmmoType GetProjectile() => _ammo;
	protected virtual void Shoot() {
		if (_firePoint == null || _ammo == null) {
			Log("TankBase - Shoot(): No firePoint");
			return;
		}
		if (_ammo.IsDrop) { _ammo.Use(transform); }
		else {
			_ammo.Use(_firePoint);
			var muzzleVfx = Instantiate(_muzzleFlash, _firePoint.position, _firePoint.rotation) as GameObject;
			muzzleVfx.transform.parent = _turret;
			Destroy(muzzleVfx, 5);
		}
	}
	public virtual void Damage(int damage) {
		_health.DecreaseHP(damage);
		if (_health.HP <= 0 && !_isDead) {
			OnDead();
			_isDead = true;
		}
	}
	protected virtual void OnDead() {
		GameObject exp = Instantiate(_explosion, transform, false) as GameObject;
		exp.transform.parent = null;
		_remains = Instantiate(_tank.Husk, transform.position, transform.rotation) as GameObject;
		Destroy(exp, 5);
	}
	protected virtual void Respawn() {
		if (_spawnPoint != null) { transform.position = _spawnPoint.position; }
		_loaded = LoadTank();
		if(!_loaded) return;
		Destroy(_remains);
		_remains = null;
		_isDead = false;
	}
	public void SetTank(TankType tank) {
		_tank = tank;
		LoadTank();
	}
	protected void ResetTank() {
		foreach (Transform child in _body) {
			Destroy(child.gameObject);
		}
		foreach (Transform child in _turret) {
			Destroy(child.gameObject);
		}
		_body.position = _turret.position = transform.position;
		transform.rotation = _body.rotation = _turret.rotation = Quaternion.identity;
	}
	protected bool LoadTank() {
		if (_tank == null) return false;
		ResetTank();
		if (_health == null) _health = GetComponent<Health>();
		_health.SetHP(_tank.MaxHealth);
		_ammo = _tank.OriginalAmmo;
		GameObject bodyObj;
		GameObject turretObj;
		if (_uiVer) { //UI menu
			bodyObj = Instantiate(_tank.UI_Hull, _body.position + _tank.BodyPosition, _tank.BodyAngle);
			turretObj = Instantiate(_tank.UI_Turret, _turret.position + _tank.TurretPosition, _tank.TurretAngle);
			return true;
		}
		else { //In game
			bodyObj = Instantiate(_tank.Hull, _body.position + _tank.BodyPosition, _tank.BodyAngle);
			turretObj = Instantiate(_tank.Turret, _turret.position + _tank.TurretPosition, _tank.TurretAngle);
		}
		bodyObj.transform.parent = _body;
		turretObj.transform.parent = _turret;
		bodyObj.transform.tag = turretObj.transform.tag = _body.tag;

		//Move fire point to child of turret cuz physics
		if (bodyObj.GetComponent<TankInfo>().firePoint) {
			_firePoint = bodyObj.transform.GetChild(0);
			_firePoint.parent = _body;
		}
		else {
			_firePoint = turretObj.transform.GetChild(0);
			_firePoint.parent = _turret;
		}
		_firePoint.rotation = Quaternion.identity;
		_canRotate = _tank.Turret.GetComponent<TankInfo>().canRotate;
		
		//Move collider from tank prefab to player object
		BoxCollider collider = GetComponent<BoxCollider>();
		BoxCollider childCol = bodyObj.transform.GetChild(0).GetComponent<BoxCollider>();
		collider.size = childCol.size;
		collider.center = childCol.center;
		transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
		Destroy(childCol);
		return true;
	}
	public void ActivateShield(float time) {
		_shield.SetActive(true);
		_shieldOn = StartCoroutine(ShieldTimer(time));
	}
	IEnumerator ShieldTimer(float time) {
		yield return new WaitForSeconds(time);
		_shieldOn = null;
		DeactivateShield();
	}
	protected void DeactivateShield() {
		if (_shieldOn != null) {
			StopCoroutine(_shieldOn);
			_shieldOn = null;
		}
		_shield.SetActive(false);
	}
	protected void Log (string message) {
		if (!_showLogs) return;
		if (_logger) _logger.Log(message, this);
		else Debug.Log(message);
	}
	protected virtual void OnDrawGizmos() {
		if (_showGizmos) {
			//Detection Range
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, _tank.DetectDist);
		}
	}
	//[System.Serializable]
	//private struct SaveData {
	//	public TankType Tank;
	//	public Health Health;
	//	public Vector3 Position;
	//	public AmmoType Ammo;
	//	public int AmmoLeft;
	//}
	//public object SaveState () {
	//	return new SaveData() {
	//		Tank = this._tank,
	//		Health = this._health,
	//		Position = this.transform.position,
	//		Ammo = this._ammo,
	//		AmmoLeft = this._ammoCountdown
	//	};
	//}
	//public void LoadState (object state) {
	//	var saveData = (SaveData)state;
	//	_tank = saveData.Tank;
	//	_health = saveData.Health;
	//	this.transform.position = saveData.Position;
	//	_ammo = saveData.Ammo;
	//	_ammoCountdown = saveData.AmmoLeft;
	//}
}