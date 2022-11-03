using System.Collections;
using UnityEngine;

public class TankControllerTest : MonoBehaviour, IDestructible {
    public static TankControllerTest Instance { get; private set; }
	[SerializeField] private TankType tank;
    [SerializeField] private Transform body, turret;
	public Transform firePoint = null;
    [SerializeField] private Health health;
    [SerializeField] private float speed;
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
    float
        projectileSpeed = 10f,
        timeToFire,
        fireRate2 = 4f,
        arcRange = 1f;

    //Unchanged
    private int gameMode, deathlessRun;
    [SerializeField] int livesRemaining;
    private bool isDead;

    private float originalFireRate;
    public float fireRate;
    private bool canFire, canMove;
    [SerializeField] bool canRotate;

    //public AudioSource audio;

    public float moveSpeed = 1.0f, turnSpeed = 25.0f, turretTurnSpeed = 25.0f;

	private Rigidbody2D rb2D;
    private float nextFire;
    private int playAlarm;
    int ammoType = 0;
    int ammoCountdown = 0;

    private float lts, rts, tlts, trts;
    public Transform GetTrans => this.transform;
    private void Awake() {
        if (!PlayerPrefs.HasKey("gameMode")) {
            PlayerPrefs.SetInt("gameMode", 3);
        }
        else {
            gameMode = PlayerPrefs.GetInt("gameMode");
        }

        if (!PlayerPrefs.HasKey("deathlessRun")) {
            PlayerPrefs.SetInt("deathlessRun", 1);
        }
        else {
            deathlessRun = PlayerPrefs.GetInt("deathlessRun");
        }
		tank = GameManager.Instance._tank;
		GameObject bodyObj = Instantiate(tank.Hull, body.position + tank.bodyPosition, tank.bodyAngle) as GameObject;
        GameObject turretObj = Instantiate(tank.Turret, turret.position + tank.turretPosition, tank.turretAngle) as GameObject;
        bodyObj.transform.parent = body;
        turretObj.transform.parent = turret;
        bodyObj.transform.tag = turretObj.transform.tag = "Player";
        firePoint = turretObj.GetComponent<TankTurretControllerTest>().firePoint;
        //unchanged
        rb2D = GetComponent<Rigidbody2D>();
    }
    private void Start() {
        //audio = GetComponent<AudioSource>();
        this.transform.position = GameController.spawnPoint;
        health.SetHP(tank.maxHealth);
        fireRate = originalFireRate = tank.fireRate;
        ammo = originalAmmo;
        canFire = true;
        canMove = true;
        playAlarm = 1;
        GameController.allyCount++;
        if (!PlayerPrefs.HasKey("livesLeft")) {
            PlayerPrefs.SetInt("livesLeft", 4);
        }
        else {
            livesRemaining = PlayerPrefs.GetInt("livesLeft");
        }
        isDead = false;
    }
    private void Update() {
        float spread = Random.Range(-0.5f + (float)health.HP / (tank.maxHealth * 2f), 0.5f - (float)health.HP / (tank.maxHealth * 2f));
        shotSpawn.localRotation = Quaternion.Euler(0, 0, spread * 1.25f);
		Debug.DrawRay(shotSpawn.position, shotSpawn.up, Color.green);

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
        if (health.LowHP) {
            if (playAlarm > 0) {
                //audio.PlayOneShot(alarm, 1.0f);
                playAlarm = 0;
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
        if (canMove) {
            //Forward movement if both triggers depressed
            if (rts > 0f && lts > 0f) { transform.Translate(moveSpeed * Time.deltaTime * Vector3.up); }
            //Backward movement if both triggers depressed
            if (rts < 0f && lts < 0f) { transform.Translate(-moveSpeed * Time.deltaTime * Vector3.up); }
            if (lts > 0f) { transform.Rotate(Vector3.forward, -turnLeft * Time.deltaTime); } //Track left
            if (rts < 0f) { transform.Rotate(Vector3.forward, turnRight * Time.deltaTime); } //Track left
            if (lts > 0f && rts < 0f) { transform.Rotate(Vector3.forward, -turnLeft * Time.deltaTime); } //Track left
            if (rts > 0f) { transform.Rotate(Vector3.forward, turnRight * Time.deltaTime); } //Track right
            if (lts < 0f) { transform.Rotate(Vector3.forward, -turnLeft * Time.deltaTime); } //Track right
            if (rts > 0f && lts < 0f) { transform.Rotate(Vector3.forward, turnRight * Time.deltaTime); } //Track right
        }
    }
    private void TurretMovement() {
		tlts = Input.GetAxis("RotateTurretL");
		trts = Input.GetAxis("RotateTurretR");

		float turnLeft = turretTurnSpeed * tlts;
		float turnRight = turretTurnSpeed * trts;

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
  //  void shoot(AudioClip sound, GameObject ammot) {
		////GetComponent<AudioSource>().PlayOneShot(sound, 1.0f);
		//muzzleAnimator.SetTrigger("Fire");
		//nextFire = Time.time + fireRate;
  //      GameObject shell = Instantiate(ammot, shotSpawn.transform, false) as GameObject;
  //      shell.transform.parent = null;

  //      if (ammoType == 2) {
  //          GameObject shell2 = Instantiate(ammot, shotSpawn.transform, false) as GameObject;
  //          GameObject shell3 = Instantiate(ammot, shotSpawn.transform, false) as GameObject;
  //          shell2.transform.localRotation = Quaternion.Euler(0f, 0f, 10f);
  //          shell3.transform.localRotation = Quaternion.Euler(0f, 0f, -10f);
  //          shell2.transform.parent = null;
  //          shell3.transform.parent = null;
  //      }
  //      ammoCountdown--;
  //  }
 //   public void SwitchAmmo(int ammot) {
 //       switch (ammot) {
 //           default:
 //               break;
 //           case 1:
 //               //GetComponent<AudioSource>().PlayOneShot(ammoSound, 1.0f);
 //               ammoType = ammot;
 //               ammoCountdown = 5;
 //               fireRate /= 2f;
 //               break;
 //           case 2:
 //               //GetComponent<AudioSource>().PlayOneShot(ammoSound, 1.0f);
 //               ammoType = ammot;
 //               ammoCountdown = 5;
 //               break;
 //           case 3:
 //               //GetComponent<AudioSource>().PlayOneShot(healthSound, 1.0f);
 //               health.IncreaseHP(5);
 //               playAlarm = 1;
 //               break;
 //           case 4:
 //               //GetComponent<AudioSource>().PlayOneShot(ammoSound, 1.0f);
 //               ammoType = ammot;
 //               ammoCountdown = 3;
 //               break;
 //           case 5:
 //               //GetComponent<AudioSource>().PlayOneShot(ammoSound, 1.0f);
 //               ammoType = ammot;
 //               ammoCountdown = 1;
 //               break;
 //       }
	//	//if (this.gameObject.CompareTag("Tank") && (collision.gameObject.CompareTag("EnemyBullet") || collision.gameObject.CompareTag("Mine"))) {
	//	//    int overfly = Random.Range(0, 9);
	//	//    if (tankSelector == 6 && overfly >= 5) { }
	//	//    else {
	//	//        GameObject ric = Instantiate(ricochet, collision.transform, false) as GameObject;
	//	//        ric.transform.parent = null;
	//	//        Destroy(collision.gameObject);
	//	//        CameraShake.Shake(0.1f, 0.1f);
	//	//        health.DecreaseHP(1);
	//	//    }
	//	//}
	//}
	public void Damage(int damage) {
        Debug.Log("IDestructible: Damage");
        VCamController.Instance.ShakeCamera(0.5f, 0.5f);
		health.DecreaseHP(damage);
		//if (health.HP <= 0 && !isDead) {
		if (health.HP <= 0) {
            OnDead();
		}
	}
    void OnDead() {
		isDead = true;
        VCamController.Instance.ShakeCamera(2f, 1f);
		GameObject exp = Instantiate(explosion, gameObject.transform, false) as GameObject;
		exp.transform.parent = null;
		GameObject remains = Instantiate(husk, transform.position, transform.rotation) as GameObject;
		remains.transform.parent = null;
		int lives = GameManager.Instance._currLives;

		if (lives > 0) {
			if (gameMode == 4) {
				deathlessRun = 0;
				PlayerPrefs.SetInt("deathlessRun", deathlessRun);
			}
			Debug.Log("You have " + lives + " lives left");
			GameManager.Instance._currLives--;
			PlayerPrefs.SetInt("livesLeft", lives);
			gameObject.transform.localScale = new Vector3(0, 0, 0);
			canFire = false;
			canMove = false;
			StartCoroutine(Respawn());
		}
		else {
			Debug.Log("No lives left");
			GameController.allyCount--;
			gameObject.SetActive(false);
			//GetComponent<AudioSource>().Stop();
		}
	}
    public void AddAmmo(GameObject AmmoType, string ammoName, int amount) {
        projectileName = ammoName;
        ammo = AmmoType;
        ammoCountdown = amount;
    }
    public string GetProjectile() => projectileName;
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
        livesRemaining--;
        health.ResetHP();
        transform.position = GameController.spawnPoint;
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        rb2D.velocity = Vector3.zero;
        rb2D.angularVelocity = 0;
        canFire = true;
        canMove = true;
        isDead = false;
	}
	public void Shoot() {
		//GetComponent<AudioSource>().PlayOneShot(ammoSound, 1.0f);
		Ray ray = new (shotSpawn.position, shotSpawn.up);
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
}