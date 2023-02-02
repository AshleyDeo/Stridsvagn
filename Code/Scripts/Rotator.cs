using UnityEngine;

public class Rotator : MonoBehaviour {
	[SerializeField] Vector3 angle = Vector3.one;
	[SerializeField] float speed = 1;
	void Update() {
		transform.Rotate(speed * Time.deltaTime * angle);
	}
}
