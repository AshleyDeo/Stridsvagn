using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
	public static SceneController Instance;
	public string CurrentScene = "";
	[SerializeField] private GameObject _loaderCanvas;
	[SerializeField] private GameObject _loaderCam;
	[SerializeField] private Slider _loadingBar;
	private bool _canPause;
	private void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else { Destroy(gameObject); }
	}
	private void Start() {
		_canPause = SceneManager.GetActiveScene().buildIndex != 0;
	}
	public async void LoadScene(string sceneName) {
		if (GameManager.Instance.Paused) UnpauseGame();
		var scene = SceneManager.LoadSceneAsync(sceneName);
		CurrentScene = sceneName;
		scene.allowSceneActivation = false;
		_loaderCanvas.SetActive(true);
		_loaderCam.SetActive(true);

		do {
			await Task.Delay(200);
			_loadingBar.value = scene.progress;
		}
		while (scene.progress < 0.9f);

		scene.allowSceneActivation = true;
		_loaderCanvas.SetActive(false);
		_loaderCam.SetActive(false);
	}
	public void ReloadScene() {
		if (GameManager.Instance.Paused) UnpauseGame();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	public void ExitGame() => Application.Quit();
	public void PauseGame() {
		if (!_canPause) return;
		Time.timeScale = 0.0f;
		GameManager.Instance.Paused = true;
	}
	public void UnpauseGame() {
		Time.timeScale = 1.0f;
		GameManager.Instance.Paused = false;
	}
	public IEnumerator Waiting(float sec) {
		yield return new WaitForSeconds(sec);
	}
}