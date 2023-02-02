using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ashspace;

public class GameControllerTest : MonoBehaviour {
    public static GameControllerTest Instance { get; private set; }
    public static int enemyCount = 0;
    public static int allyCount = 0;

    //unchenged
    [SerializeField] private PauseGame pauseMenu;

    public Text gameText;
    public Text enemiesLeftText;

    public GameObject gameOverMenu;
    public TMP_Text gameOverText;

    void OnEnable() {
        PlayerTank.OnPlayerDead += ActivateGameOverMenu;
    }
    void OnDisable() {
        PlayerTank.OnPlayerDead -= ActivateGameOverMenu;
    }
    private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(this);
		}
		else {
			Instance = this;
		}
    }
    public int CountEnemies() => enemyCount;
    public int CountAllies() => allyCount;
    public void ActivateGameOverMenu(bool isDead) {
        pauseMenu.Paused();
        gameOverMenu.SetActive(true);
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