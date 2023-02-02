using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTurretControllerTest : MonoBehaviour {
	public Animator anim = null;
	public Transform firePoint = null;
	[SerializeField] private bool canRotate;

	[SerializeField] private LineRenderer laserSight;
}