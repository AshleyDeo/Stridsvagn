using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
	//private int currentScene;
	[SerializeField] private int nextScene = 0;
	private bool canPause;
	private void Start() {
		canPause = SceneManager.GetActiveScene().buildIndex != 0;
	}
	public void LoadScene(int scene) {
		if (GameManager.Instance._paused) UnpauseGame();
		SceneManager.LoadScene(scene);
	}
	public void SetNextScene(int scene) => nextScene = scene;
	public void LoadNextScene() {
		if (GameManager.Instance._paused) UnpauseGame();
		StartCoroutine(Waiting(1f));
		SceneManager.LoadScene(nextScene);
	}
	public void ReloadScene() {
		if (GameManager.Instance._paused) UnpauseGame();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	public void ExitGame() => Application.Quit();
	public void PauseGame() {
		if (!canPause) return;
		Time.timeScale = 0.0f;
		GameManager.Instance._paused = true;
	}
	public void UnpauseGame() {
		Time.timeScale = 1.0f;
		GameManager.Instance._paused = false;
	}
	IEnumerator Waiting(float sec) {
		yield return new WaitForSeconds(sec);
	}
}