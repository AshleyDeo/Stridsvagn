using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveStraightLine : MonoBehaviour, IDestructible {
    public Transform bumper;
	public Health Health { get; private set; }

	public float speed;
    public bool randomRotation;
    public int rotation;
    public bool canMove;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    void Awake() {
        //xMin = GameControllerTest.Instance.xMin;
        //xMax = GameControllerTest.Instance.xMax;
        //yMin = GameControllerTest.Instance.yMin;
        //yMax = GameControllerTest.Instance.yMax;
		Health = GetComponent<Health>();
		Health.SetHP(20);
	}
    void Start() {
        if (randomRotation) rotation = Random.Range(0, 10);
        if (rotation < 5) transform.rotation = Quaternion.Euler(0, 0, 0);
        else transform.rotation = Quaternion.Euler(0, 0, -90);
    }
    private void FixedUpdate() {
        if (!canMove) return;
        transform.Translate(speed * Time.deltaTime * transform.up, Space.World);

        RaycastHit2D guard = Physics2D.BoxCast(bumper.position, new (0.67f, 0.67f), 0.0f, transform.forward, 0.0f, 13 << 6);

        if (guard.collider != null) {
            transform.localRotation *= Quaternion.Euler(0, 0, 180);
        }
	}
	public void Damage(int damage) {
		Health.DecreaseHP(damage);
	}
	void OnDrawGizmos() {
		Gizmos.color = Color.gray;
		Gizmos.DrawCube(bumper.position, new(0.67f, 0.67f,1f));
	}
}