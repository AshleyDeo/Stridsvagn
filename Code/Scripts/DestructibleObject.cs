using UnityEngine;

[RequireComponent(typeof(Health))]
public class DestructibleObject : MonoBehaviour, IDestructible {
	[SerializeField] private Health _health = null;
	[SerializeField] private int _healthPoints;
	[SerializeField] private GameObject _explosion;
	[SerializeField] private AudioClip _destroyAudio;
	[SerializeField] private GameObject _alive;
	[SerializeField] private GameObject _remains;

	void Start() {
		_health = GetComponent<Health>();
		_health.SetHP(_healthPoints);
	}
	public void Damage(int damage) {
		_health.DecreaseHP(damage);
		if (_health.HP <= 0) {
			OnDead();
			Destroy(gameObject);
		}
	}
	void OnDead() {
		SoundManager.Instance.PlaySound(_destroyAudio);
		var impactVfx = Instantiate(_explosion, transform.position, Quaternion.identity) as GameObject;
		_remains.SetActive(true);
		_alive.SetActive(false);
		Destroy(impactVfx, 5);
	}
}