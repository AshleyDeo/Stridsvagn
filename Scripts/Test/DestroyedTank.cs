using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedTank : MonoBehaviour {
	[SerializeField] private TankType tank;
	void Awake() {
		GameObject bodyObj = Instantiate(tank.Hull, transform.position + tank.bodyPosition, tank.bodyAngle) as GameObject;
		bodyObj.transform.parent = transform;
	}
}