using UnityEngine;

[CreateAssetMenu(fileName = "New Tank", menuName = "Assets/Tank")]
public class TankType : ScriptableObject {
	[Header("Design")]
	public GameObject Hull;
	public GameObject Turret;
	public GameObject Husk;
	public string OfficialName;
	public string Codename;
	public string Flavor;
	public string Bonus;
	[Header("Stats")]
	public int MaxHealth; 
	public AmmoType OriginalAmmo;
	public float DetectDist;
	public float FireRate;
	public float MoveSpeed;
	public float TurnSpeed;
	public float TurretRotSpeed;
	[Header("Unlock")]
	public string HowToUnlock;
	public int Kills;
	public bool Campaign;
	public bool ZeroDeaths;
	[Header("UI Visual")]
	public GameObject UI_Hull;
	public GameObject UI_Turret;
	[Header("Reposition")]
	public Vector3 BodyPosition;
	public Quaternion BodyAngle;
	public Vector3 TurretPosition;
	public Quaternion TurretAngle;
}