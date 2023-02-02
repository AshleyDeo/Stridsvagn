using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedTank : MonoBehaviour {
	[SerializeField] private TankType _tank;
	void Awake() {
		GameObject bodyObj = Instantiate(_tank.Hull, transform.position + _tank.BodyPosition, _tank.BodyAngle) as GameObject;
		bodyObj.transform.parent = transform;
	}
}