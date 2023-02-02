using UnityEngine;

namespace ashspace
{
    public class Spawner : MonoBehaviour {
        [SerializeField] private GameObject _cratePrefab;
        [SerializeField] private Transform _crateParent;
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private Transform _enemyParent;
        [SerializeField] private GameObject _allyPrefab;
        [SerializeField] private Transform _allyParent;

        private LevelInfo _levelInfo;
        void Start() {
            _levelInfo = GameManager.Instance.CurrentLevel;
            LoadCrates();
            LoadEnemies();
            LoadAllies();
        }
        void LoadCrates() {
            for (int i = 0; i < _levelInfo.NumCrates; i++) {
                GameObject crate = Instantiate(_cratePrefab, _crateParent) as GameObject;
                crate.transform.position= new(Random.Range(_levelInfo.MinLocation.x, _levelInfo.MaxLocation.x), 0.5f, 
                    Random.Range(_levelInfo.MinLocation.y, _levelInfo.MaxLocation.y));
			}
			Debug.Log("Crates Loaded");
		}
        void LoadEnemies() {
			for (int i = 0; i < _levelInfo.NumEnemies; i++) {
				GameObject enemy = Instantiate(_enemyPrefab, _enemyParent) as GameObject;
                EnemyTank tank = enemy.GetComponent<EnemyTank>();
                tank.SetTank(_levelInfo.EnemyTypes[Random.Range(0, _levelInfo.EnemyTypes.Length)]);
                int loops = 0;
                Collider[] hitColliders;
                do {
                    loops++;
                    enemy.transform.position = new(Random.Range(_levelInfo.MinLocation.x, _levelInfo.MaxLocation.x), 0f, 
                        Random.Range(_levelInfo.MinLocation.y, _levelInfo.MaxLocation.y));
                    hitColliders = Physics.OverlapSphere(enemy.transform.position, 2f, 6 << 8);
                } while (hitColliders.Length > 0 || loops < 100);
			}
            Debug.Log("Enemies Loaded");
		}
        void LoadAllies() {
			Debug.Log("Allies Loaded");
		}
    }
}