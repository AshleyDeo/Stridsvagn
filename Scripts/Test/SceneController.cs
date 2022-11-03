using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
	private int currentScene;
	private bool canPause;
	void LoadScene(int scene) {
		SceneManager.LoadScene(scene);
	}
	private void Start() {
		if (SceneManager.GetActiveScene().buildIndex == 1) {
			canPause = false;
		}
		else {
			canPause = true;
		}
	}
}