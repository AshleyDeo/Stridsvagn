using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tank", menuName = "Assets/Tanks")]
public class TankType : ScriptableObject {
	[Header("Design")]
	public GameObject Hull;
	public GameObject Turret;
	public GameObject Husk;
	public string officialName;
	public string codename;
	public string flavor;
	public string bonus;
	[Header("Stats")]
	public int maxHealth;
	public float detectDist;
	public float fireRate;
	public float moveSpeed;
	public float turnSpeed;
	public float turretRotSpeed;
	[Header("Unlock")]
	public string howToUnlock;
	public int kills;
	public bool campaign;
	public bool zeroDeaths;
	[Header("Reposition")]
	public Vector3 bodyPosition;
	public Quaternion bodyAngle;
	public Vector3 turretPosition;
	public Quaternion turretAngle;
}