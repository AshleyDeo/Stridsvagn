using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveStraightLine : MonoBehaviour
{
    public Transform bumper;
    private GameControllerTest gameController;
    //private GameController gameController;

	public float speed;
    public bool randomRotation;
    public int rotation;
    public bool canMove;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    void Start()
    {
        if (randomRotation) rotation = Random.Range(0, 10);
        if (rotation < 5) transform.rotation = Quaternion.Euler(0, 0, 0);
        else transform.rotation = Quaternion.Euler(0, 0, -90);
        //transform.localRotation = Quaternion.Euler(0, 0, (float)rotation);
        gameController = GameObject.Find("GameController").GetComponent<GameControllerTest>();
        xMin = gameController.xMin;
        xMax = gameController.xMax;
        yMin = gameController.yMin;
        yMax = gameController.yMax;
    }
    private void FixedUpdate() {
        if (canMove) {
            transform.Translate(speed * Time.deltaTime * transform.up, Space.World);

            RaycastHit2D guard = Physics2D.BoxCast(bumper.position, new Vector2(0.67f, 0.67f), 0.0f, transform.forward, 0.0f, 13 << 6);

            if (transform.position.x <= xMin ||
                transform.position.x >= xMax ||
                transform.position.y <= yMin ||
                transform.position.y >= yMax ||
                guard.collider != null) {
                transform.localRotation *= Quaternion.Euler(0, 0, 180);
            }
        }
    }
}
