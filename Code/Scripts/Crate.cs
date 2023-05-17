using strids;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Crate : MonoBehaviour, IDestructible {
    private Health _health;
    [SerializeField] private int healthPoints;
    [SerializeField] private GameObject _broken;
    [SerializeField] private PowerupEffect[] _powerups;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject _explosion;
	[SerializeField] private AudioClip _destroyAudio;

    private Vector2 _minLocation;
    private Vector2 _maxLocation;

	void Awake() {
		_health = GetComponent<Health>();
		_health.SetHP(healthPoints);
    }
    void Start() {
        LevelInfo level = GameManager.Instance.CurrentLevel; 
        _minLocation = level.MinLocation;
        _maxLocation = level.MinLocation;
    }
	public void Damage(int damage) {
		_health.DecreaseHP(damage);
        if (_health.HP <= 0) OnDead();
	}
	void OnDead() {
		SoundManager.Instance.PlaySound(_destroyAudio);
		int chosen = Random.Range(0, _powerups.Length);
        GameObject broke = Instantiate(_broken, transform.position, _broken.transform.rotation) as GameObject;
        broke.GetComponent<Pickup>().Powerup = _powerups[chosen];
        GameObject pickup = Instantiate(_powerups[chosen].Drop, transform.position + new Vector3(0,-0.15f,0), _broken.transform.rotation) as GameObject;
        pickup.transform.parent = broke.transform;
        broke.transform.Rotate(180f, 0f, 0f);
        pickup.transform.Rotate(-90f, 0f, 0f);
        transform.position= new(0, -20f, 0);
        StartCoroutine(Respawn(Random.Range(10f, 30f)));
	}
    IEnumerator Respawn(float timer) {
        yield return new WaitForSeconds(timer);
        transform.position = new(Random.Range(_minLocation.x, _maxLocation.x), 0.5f,
        Random.Range(_minLocation.y, _maxLocation.y));
		_health.ResetHP();
    }
}