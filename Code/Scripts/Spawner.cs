using UnityEngine;

namespace strids
{
    public class Spawner : MonoBehaviour {
        [SerializeField] private Transform _propParent;
        [SerializeField] private GameObject _cratePrefab;
        [SerializeField] private Transform _crateParent;
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private Transform _enemyParent;
        [SerializeField] private GameObject _allyPrefab;
        [SerializeField] private Transform _allyParent;

        private LevelInfo _levelInfo;
        [SerializeField] private LayerMask _propMask;
        [SerializeField] private LayerMask _tankMask;
        [SerializeField] private LayerMask _crateMask;
        void Start() {
            _levelInfo = GameManager.Instance.CurrentLevel;
            LoadProps();
            LoadCrates();
            LoadEnemies();
            //LoadAllies();
        }
        void LoadProps() {
            int numBuildings = Random.Range(7, 20);
            for (int i = 0; i < numBuildings; i++) {
                Transform prop = Instantiate(_levelInfo.Buildings[Random.Range(0, _levelInfo.Buildings.Length)], _propParent).transform;
				BoxCollider bc = prop.GetComponent<BoxCollider>();
				int loops = 0;
				Collider[] hitColliders;
                do {
                    loops++;
                    prop.position = new(Random.Range(_levelInfo.MinLocation.x, _levelInfo.MaxLocation.x), 0f,
                    Random.Range(_levelInfo.MinLocation.y, _levelInfo.MaxLocation.y));
					hitColliders = Physics.OverlapBox(prop.position, bc.size, Quaternion.identity, _propMask);
				} while (hitColliders.Length > 1 && loops < 100);
                //Debug.Log($"Props Looped: {loops}");
			}
			Debug.Log("Props Loaded");
		}
        void LoadCrates() {
            for (int i = 0; i < _levelInfo.NumCrates; i++) {
                Transform crate = Instantiate(_cratePrefab, _crateParent).transform;
				BoxCollider bc = crate.GetComponent<BoxCollider>();
				int loops = 0;
				Collider[] hitColliders;
                do {
                    loops++;
                    crate.position = new(Random.Range(_levelInfo.MinLocation.x, _levelInfo.MaxLocation.x), 0.5f,
                    Random.Range(_levelInfo.MinLocation.y, _levelInfo.MaxLocation.y));
					hitColliders = Physics.OverlapBox(crate.position, bc.size*1.3f, Quaternion.identity, _crateMask);
				} while (hitColliders.Length > 1 && loops < 100);
				//Debug.Log($"Crates Looped: {loops}");
			}
			Debug.Log("Crates Loaded");
		}
        void LoadEnemies() {
			for (int i = 0; i < _levelInfo.NumEnemies; i++) {
				Transform enemy = Instantiate(_enemyPrefab, _enemyParent).transform;
                EnemyTank tank = enemy.GetComponent<EnemyTank>();
                tank.SetTank(_levelInfo.EnemyTypes[Random.Range(0, _levelInfo.EnemyTypes.Length)]);
                int loops = 0;
                Collider[] hitColliders;
                do {
                    loops++;
                    enemy.position = new(Random.Range(_levelInfo.MinLocation.x, _levelInfo.MaxLocation.x), 0f, 
                        Random.Range(2f, _levelInfo.MaxLocation.y));
                    hitColliders = Physics.OverlapSphere(enemy.position, 2f, _tankMask);
                } while (hitColliders.Length > 1 && loops < 100);
			}
            Debug.Log("Enemies Loaded");
		}
        void LoadAllies() {
			for (int i = 0; i < _levelInfo.NumEnemies; i++) {
				GameObject enemy = Instantiate(_enemyPrefab, _enemyParent) as GameObject;
				TankBase tank = enemy.GetComponent<TankBase>();
				tank.SetTank(_levelInfo.AllyTypes[Random.Range(0, _levelInfo.EnemyTypes.Length)]);
				int loops = 0;
				Collider[] hitColliders;
				do {
					loops++;
					enemy.transform.position = new(Random.Range(_levelInfo.MinLocation.x, _levelInfo.MaxLocation.x), 0f,
						Random.Range(_levelInfo.MinLocation.y, -2f));
					hitColliders = Physics.OverlapSphere(enemy.transform.position, 2f, _tankMask);
				} while (hitColliders.Length > 1 || loops < 100);
			}
			Debug.Log("Allies Loaded");
		}
    }
}