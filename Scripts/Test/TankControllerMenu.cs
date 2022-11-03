using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class TankControllerMenu : MonoBehaviour {
    [SerializeField] private CharacterMainMenuTest _menu;
    [SerializeField] private Transform body, turret;
	[SerializeField] private List<Transform> firePoints;
	[SerializeField] private Transform firePoint;
	[SerializeField] private Health health;
    [SerializeField] private float speed;
	private Animator muzzleAnimator = null;
	[SerializeField] private GameObject muzzleFlash;
	//Powerups
	[SerializeField] private GameObject originalAmmo, ammo;
	[SerializeField] private PowerupEffect powerup = null;
	[SerializeField] private Transform shotSpawn;
	[SerializeField] private GameObject shield, explosion, husk;
	private Vector3 destination;
	private string projectileName = "";
	//Audio
	[SerializeField] private AudioClip shotSound;
    [SerializeField] private AudioClip alarm;
	//Laser
	[SerializeField] private LineRenderer laserSight;
	//Unchenged
	float
		projectileSpeed = 10f,
		timeToFire,
		fireRate2 = 4f,
		arcRange = 1f;

	private int gameMode, deathlessRun, tankSelector;
    public int livesRemaining;
    private bool isDead;

    [SerializeField] Texture Box;
    public float fireRate;
    private float originalFireRate;
	private bool canFire, canMove;
	[SerializeField] bool canRotate;

	//public AudioSource audio;

	public float moveSpeed = 1.0f, turnSpeed = 25.0f, turretTurnSpeed = 25.0f;

	private Rigidbody2D rb2D;
    private float nextFire;
    private int playAlarm;
    public int ammoType;
    private int ammoCountdown;

	private float lts, rts, tlts, trts;

	private void Awake() {
		rb2D = GetComponent<Rigidbody2D>();
		foreach (TankType tank in _menu.tanks) {
			GameObject bodyObj = Instantiate(tank.Hull, body.position + tank.bodyPosition, tank.bodyAngle) as GameObject;
			GameObject turretObj = Instantiate(tank.Turret, turret.position + tank.turretPosition, tank.turretAngle) as GameObject;
			bodyObj.transform.parent = body;
			turretObj.transform.parent = turret;
			Transform point = turretObj.GetComponent<TankInfo>().firePoint;
			if (point == null) {
				point = bodyObj.GetComponent<TankInfo>().firePoint;
			}
			firePoints.Add(point);
		}
		firePoint = firePoints[0];
    }
    private void Start() {
        this.transform.position = GameController.spawnPoint;
        health.SetHP(100);
		//audio = GetComponent<AudioSource>();
		fireRate = originalFireRate = 4f;
		ammo = originalAmmo;
		playAlarm = 1;
        ammoType = 0;
        ammoCountdown = 0;
        GameController.allyCount++;
        originalFireRate = fireRate;
        canFire = true;
        canMove = true;
        if (!PlayerPrefs.HasKey("livesLeft")) {
            PlayerPrefs.SetInt("livesLeft", 4);
        }
        else {
            livesRemaining = PlayerPrefs.GetInt("livesLeft");
        }
        isDead = false;
    }
    private void Update() {
        float spread = Random.Range(0.5f, 1f);
        shotSpawn.localRotation = Quaternion.Euler(0, 0, spread * 1.25f);

        if (ammoCountdown <= 0) {
            ammoType = 0;
            ammo = originalAmmo;
			projectileName = "";
            fireRate = originalFireRate;
        }
        if ((Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) && Time.time > nextFire && canFire) {
            //if (powerup) { powerup.Apply(this.gameObject); }
            //else { Shoot(); }
            Projectile projectile = ammo.GetComponent<Projectile>();
            if (projectile != null) {
                Shoot();
            }
            else {
                DropMine();
            }
        }
	}
	private void FixedUpdate() {
		TankMovement();
		TurretMovement();
	}
	private void TankMovement() {
		lts = Input.GetAxis("Vertical");
		rts = Input.GetAxis("Vertical2");

		float turnLeft = turnSpeed * lts;
		float turnRight = turnSpeed * rts;
		//Forward movement if both triggers depressed
		if (rts > 0f && lts > 0f && canMove) { transform.Translate(Vector3.up * moveSpeed * Time.deltaTime); }
		//Backward movement if both triggers depressed
		if (rts < 0f && lts < 0f && canMove) { transform.Translate(Vector3.up * -moveSpeed * Time.deltaTime); }
		if (lts > 0f && canMove) { transform.Rotate(Vector3.forward, -turnLeft * Time.deltaTime); } //Track left
		if (rts < 0f && canMove) { transform.Rotate(Vector3.forward, turnRight * Time.deltaTime); } //Track left
		if (lts > 0f && rts < 0f && canMove) { transform.Rotate(Vector3.forward, -turnLeft * Time.deltaTime); } //Track left
		if (rts > 0f && canMove) { transform.Rotate(Vector3.forward, turnRight * Time.deltaTime); } //Track right
		if (lts < 0f && canMove) { transform.Rotate(Vector3.forward, -turnLeft * Time.deltaTime); } //Track right
		if (rts > 0f && lts < 0f && canMove) { transform.Rotate(Vector3.forward, turnRight * Time.deltaTime); } //Track right
	}
	private void TurretMovement() {
		tlts = Input.GetAxis("RotateTurretL");
		trts = Input.GetAxis("RotateTurretR");

		float turnLeft = turretTurnSpeed * tlts;
		float turnRight = turretTurnSpeed * trts;
		canRotate = _menu.tanks[_menu.tankSelector].Turret.GetComponent<TankInfo>().canRotate;
		if (!canRotate) turret.localRotation = Quaternion.Euler(Vector3.forward);
		if (tlts > 0f && canRotate) { turret.Rotate(Vector3.forward, turnLeft * Time.deltaTime); } //Rotate left
		if (trts > 0f && canRotate) { turret.Rotate(Vector3.forward, -turnRight * Time.deltaTime); } //Rotate right

		RaycastHit2D sight = Physics2D.Raycast(shotSpawn.position, turret.up, 7.0f, 3 << 8);
		/*if (sight.collider != null) {
			//Debug.Log(sight.collider.name);
			laserSight.enabled = true;
			Vector3[] positions = new Vector3[2] { shotSpawn.position, (Vector3)sight.point };
			//laserSight.positionCount = 2;
			laserSight.startWidth = 0.025f;
			laserSight.endWidth = 0.025f;
			laserSight.SetPositions(positions);
		}
		else {
			laserSight.enabled = false;
		}*/
	}
	public void AddAmmo(GameObject AmmoType, string ammoName, int amount) {
		projectileName = ammoName;
		ammo = AmmoType;
		ammoCountdown = amount;
	}
	public string GetProjectile() => projectileName;
	public void Shoot() {
		//GetComponent<AudioSource>().PlayOneShot(ammoSound, 1.0f);
		Ray ray = new(shotSpawn.position, shotSpawn.up);
		if (Physics.Raycast(ray, out RaycastHit hit)) {
			destination = hit.point;
		}
		else {
			destination = ray.GetPoint(10000);
		}
		InstantiateProjectile();
		var muzzleVfx = Instantiate(muzzleFlash, firePoint.position, firePoint.rotation) as GameObject;
		muzzleVfx.transform.parent = turret;
		Destroy(muzzleVfx, 5);
		if (ammoCountdown > 0) { ammoCountdown--; }
	}
	void DropMine() {
        GameObject laidMine = Instantiate(ammo, this.transform, false) as GameObject;
        laidMine.transform.parent = null;
        if (ammoCountdown > 0) ammoCountdown--;
	}
	public void ActivateShield() {
		shield.SetActive(true);
		StartCoroutine(DeactivateShield());
	}
	IEnumerator DeactivateShield() {
        yield return new WaitForSeconds(10.0f);
        shield.SetActive(false);
    }
    IEnumerator Respawn() {
        yield return new WaitForSeconds(2.0f);

        health.ResetHP();
        transform.position = GameController.spawnPoint;
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        rb2D.velocity = Vector3.zero;
        rb2D.angularVelocity = 0;
        canFire = true;
        canMove = true;
        isDead = false;
	}
	void InstantiateProjectile() {
		var projectileObject = Instantiate(ammo, shotSpawn.position, shotSpawn.rotation) as GameObject;
		projectileObject.GetComponent<Rigidbody2D>().velocity = (destination - shotSpawn.position).normalized * projectileSpeed;
		//iTween.PunchPosition(projectileObject, new Vector3(
		//	Random.Range(-arcRange, arcRange), Random.Range(-arcRange, arcRange), 0),  Random.Range(0.5f, 2));
	}
	void OnCollisionEnter2D(Collision2D collision) {
		//Debug.Log("Child collider works");
		Pickup pickup = collision.gameObject.GetComponent<Pickup>();
		if (pickup != null) {
			//powerup = pickup;
			pickup.powerup.Apply(this.gameObject);
			Destroy(collision.gameObject);
		}
	}
	public void SelectTank() {
        foreach (Transform tank in body) {
            tank.gameObject.SetActive(false);
        }
        foreach (Transform tank in turret) {
            tank.gameObject.SetActive(false);
        }
		int index = _menu.tankSelector;
        body.GetChild(index).gameObject.SetActive(true);
        turret.GetChild(index+2).gameObject.SetActive(true);
		firePoint = firePoints[index];
	}
}