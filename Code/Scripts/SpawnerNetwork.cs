using UnityEngine;
using Unity.Netcode;

namespace strids {
    public class SpawnerNetwork : NetworkBehaviour {
        public static SpawnerNetwork Instance { get; private set; }
        [SerializeField] private Transform _propParent;
        [SerializeField] private GameObject _cratePrefab;
        [SerializeField] private Transform _crateParent;
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private Transform _enemyParent;
        [SerializeField] private GameObject _allyPrefab;
        [SerializeField] private Transform _allyParent;

        private LevelInfo _levelInfo;
		private void Awake () {
			if (Instance != null && Instance != this) {
				Destroy(gameObject);
			}
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		private void Start () {
            _levelInfo = GameManager.Instance.CurrentLevel;
            Debug.Log(_levelInfo.name);
        }
        [ServerRpc]
        public void LoadPropsServerRpc () {
            int numBuildings = Random.Range(10, 30);
            for (int i = 0; i < numBuildings; i++) {
                GameObject prop = Instantiate(_levelInfo.Buildings[Random.Range(0, _levelInfo.Buildings.Length)], _propParent) as GameObject;
                prop.transform.position = new(Random.Range(_levelInfo.MinLocation.x, _levelInfo.MaxLocation.x), 0f,
                    Random.Range(_levelInfo.MinLocation.y, _levelInfo.MaxLocation.y));
                //LoadPropsClientRpc(prop);
            }
            Debug.Log("Props Loaded");
        }
        [ClientRpc]
        void LoadPropsClientRpc () {
            //Instantiate(prop);
        }
		[ServerRpc]
        public void LoadCratesServerRpc () {
            for (int i = 0; i < _levelInfo.NumCrates; i++) {
                GameObject crate = Instantiate(_cratePrefab, _crateParent) as GameObject;
                crate.transform.position = new(Random.Range(_levelInfo.MinLocation.x, _levelInfo.MaxLocation.x), 0.5f,
                    Random.Range(_levelInfo.MinLocation.y, _levelInfo.MaxLocation.y));
                //LoadCratesClientRpc(crate);
            }
            Debug.Log("Crates Loaded");
		}
		[ClientRpc]
		void LoadCratesClientRpc () {
			//Instantiate(crate);
		}
        public void LoadEnemies () {
            for (int i = 0; i < _levelInfo.NumEnemies; i++) {
                GameObject enemy = Instantiate(_enemyPrefab) as GameObject;
                TankBase tank = enemy.GetComponent<TankBase>();
                tank.SetTank(_levelInfo.EnemyTypes[Random.Range(0, _levelInfo.EnemyTypes.Length)]);
                int loops = 0;
                Collider[] hitColliders;
                do {
                    loops++;
                    enemy.transform.position = new(Random.Range(_levelInfo.MinLocation.x, _levelInfo.MaxLocation.x), 0f,
                        Random.Range(2f, _levelInfo.MaxLocation.y));
                    hitColliders = Physics.OverlapSphere(enemy.transform.position, 2f, 6 << 8);
                } while (hitColliders.Length > 0 || loops < 100);
                NetworkObject e = enemy.GetComponent<NetworkObject>();
                e.Spawn(true);
            }
            Debug.Log("Enemies Loaded");
		}
		[ClientRpc]
		void LoadEnemiesClientRpc () {
			//Instantiate(enemy);
		}
		void LoadAllies () {
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
                    hitColliders = Physics.OverlapSphere(enemy.transform.position, 2f, 6 << 8);
                } while (hitColliders.Length > 0 || loops < 100);
            }
            Debug.Log("Allies Loaded");
        }
    }
}