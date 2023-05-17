using UnityEngine;
using Unity.Netcode;
using TMPro;
namespace strids {
    public class GameControllerNetwork : NetworkBehaviour {
        public static GameControllerNetwork Instance { get; private set; }

        [SerializeField] private int allyCount = 0;
        [SerializeField] private int enemyCount = 0;
        [SerializeField] private PauseGame pauseMenu;
        [SerializeField] private GameState State;

        public TMP_Text enemiesLeftText;

        public Canvas gameOverMenu;
        public TMP_Text gameOverText;

        void OnEnable () {
            PlayerTank.OnPlayerDead += ActivateGameOverMenu;
            EnemyTank.EnemyLive += AddEnemiesServerRpc;
            EnemyTank.EnemyDead += RemoveEnemiesServerRpc;
        }
        void OnDisable () {
            PlayerTank.OnPlayerDead -= ActivateGameOverMenu;
            EnemyTank.EnemyLive -= AddEnemiesServerRpc;
            EnemyTank.EnemyDead -= RemoveEnemiesServerRpc;
        }
        private void Awake () {
            if (Instance != null && Instance != this) {
                Destroy(this);
            }
            else {
                Instance = this;
            }
            State = GameState.Waiting;
        }
		void Update () {
            if (!IsServer) return;
			switch(State) {
                case GameState.Waiting:
                    break;
                case GameState.Loading:
					//SpawnerNetwork.Instance.LoadPropsServerRpc();
					//SpawnerNetwork.Instance.LoadCratesServerRpc();
					//SpawnerNetwork.Instance.LoadEnemies();
                    State = GameState.GameStarted;
					break;
                case GameState.GameStarted:
                    break;
                case GameState.GameOver:
                    break;
            }
		}
		[ServerRpc]
        void AddEnemiesServerRpc () {
            enemyCount++;
        }
        [ServerRpc]
        void RemoveEnemiesServerRpc () {
            enemyCount--;
            if (enemyCount == 0) ActivateGameOverMenu(false);
        }
        public int CountEnemies () => enemyCount;
        public int CountAllies () => allyCount;
        public void ActivateGameOverMenu (bool isDead) {
            State = GameState.GameOver;
            Time.timeScale = 0;
            gameOverMenu.enabled = true;
            GameManager.Instance.CurrLives--;
            if (isDead) {
                gameOverText.text = "Defeated";
                gameOverText.color = Color.red;
            }
            else {
                gameOverText.text = "Victory";
                gameOverText.color = Color.green;
            }
        }
        public void FinishLoading () => State = GameState.GameStarted;
    }
    public enum GameState {
        Waiting,
        Loading,
        GameStarted,
        GameOver
    }
}