using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace strids {
    public class GameOverMenu : MonoBehaviour {
		[SerializeField] Canvas gameOverMenu;
		[SerializeField] TMP_Text gameOverText;
        [SerializeField] Button nextLevel;

		void OnEnable() {
			PlayerTank.OnPlayerDead += ActivateGameOverMenu;
		}
		void OnDisable() {
			PlayerTank.OnPlayerDead -= ActivateGameOverMenu;
		}
		void Awake() {
			gameOverMenu = GetComponent<Canvas>();
		}
		public void ActivateGameOverMenu(bool isDead) {
			Time.timeScale = 0;
			gameOverMenu.enabled = true;
			GameManager.Instance.CurrLives--;
			if (isDead) {
				gameOverText.text = "Defeated";
				gameOverText.color = Color.red;
				nextLevel.interactable = false;
			}
			else {
				gameOverText.text = "Victory";
				gameOverText.color = Color.green;
				nextLevel.interactable = true;
			}
		}
	}
}