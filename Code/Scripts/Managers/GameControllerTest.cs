using UnityEngine;
using TMPro;
using strids;

public class GameControllerTest : MonoBehaviour {
    public static GameControllerTest Instance { get; private set; }

    [SerializeField] private int allyCount = 0;
    [SerializeField] private int enemyCount = 0;
    [SerializeField] private PauseGame pauseMenu;

    public TMP_Text enemiesLeftText;

    public Canvas gameOverMenu;
    public TMP_Text gameOverText;

    void OnEnable() {
        PlayerTank.OnPlayerDead += ActivateGameOverMenu;
        EnemyTank.EnemyLive += AddEnemies;
        EnemyTank.EnemyDead += RemoveEnemies;
    }
    void OnDisable() {
        PlayerTank.OnPlayerDead -= ActivateGameOverMenu;
		EnemyTank.EnemyLive -= AddEnemies;
		EnemyTank.EnemyDead -= RemoveEnemies;
	}
    private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(this);
		}
		else {
			Instance = this;
		}
    }
    void AddEnemies() {
        enemyCount++;
    }
    void RemoveEnemies() {
        enemyCount--;
        if(enemyCount == 0) ActivateGameOverMenu(false);
    }
    public int CountEnemies() => enemyCount;
    public int CountAllies() => allyCount;
    public void ActivateGameOverMenu(bool isDead) {
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
}