using Unity.VisualScripting;
using UnityEngine;
using static Cinemachine.CinemachineConfiner;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;
using static UnityEngine.Rendering.DebugUI;

public class TankCreator : MonoBehaviour, IDestructible {
	[SerializeField] private TankType _tank = null;
	[SerializeField] private Transform body;
	public Transform turret;
	[SerializeField] private GameObject explosion;
	public Health Health { get; private set; }
	public Transform firePoint = null;
	public bool isDead = false;
	public TankType GetTank => _tank;
	public void CreateTank(TankType tank) {
		_tank = tank;
		Health = GetComponent<Health>();
		Health.SetHP(tank.maxHealth);
		GameObject bodyObj = Instantiate(tank.Hull, body.position + tank.bodyPosition, tank.bodyAngle) as GameObject;
		GameObject turretObj = Instantiate(tank.Turret, turret.position + tank.turretPosition, tank.turretAngle) as GameObject;
		bodyObj.transform.parent = body;
		turretObj.transform.parent = turret;
		bodyObj.transform.tag = turretObj.transform.tag = "Player";
		Transform point = turretObj.GetComponent<TankInfo>().firePoint;
		if (point == null) {
			point = bodyObj.GetComponent<TankInfo>().firePoint;
		}
		firePoint = point;
		TankMover mover = this.GetComponent<TankMover>();
		mover.SetRoate(_tank.Turret.GetComponent<TankInfo>().canRotate);
	}
	public void Damage(int damage) {
		Debug.Log("IDestructible: Damage");
		//VCamController.Instance.ShakeCamera(0.5f, 0.5f);
		Health.DecreaseHP(damage);
		if (Health.HP <= 0) {
			OnDead();
		}
	}
	void OnDead() {
		GameObject exp = Instantiate(explosion, gameObject.transform, false) as GameObject;
		exp.transform.parent = null;
		GameObject remains = Instantiate(_tank.Husk, transform.position, transform.rotation) as GameObject;
		remains.transform.parent = null;
	}
	//void DropMine(GameObject ammo) {
	//	GameObject laidMine = Instantiate(ammo, this.transform, false) as GameObject;
	//	laidMine.transform.parent = null;
	//}
}