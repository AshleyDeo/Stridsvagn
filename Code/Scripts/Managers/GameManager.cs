using UnityEngine;
using strids;

public class GameManager : MonoBehaviour, ISaveable {
	public static GameManager Instance { get; private set; }

	//Events
	//public static event Action<GameState> OnBeforeStateChanged;
	//public static event Action<GameState> OnAfterStateChanged;
	//public static GameState State { get; private set; }

	public LevelInfo CurrentLevel = null;
	public TankType Tank = null;
	public int LevelsCompleted = 0;
	public int MaxLevels = 5;
	public int Kills = 0;
	public int Deaths = int.MaxValue;
	public bool Campaign = false;
	public int CurrLives = 5;

	[Header("Loader")]
	public GameMode Mode = GameMode.Menu;
	public int NextScene = 0;
	public bool Paused = false;

	//private bool _canPause;

	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
	}
	void Start() {
		SoundManager.Instance.PlayMusic(CurrentLevel.Music);
		Time.timeScale = (CurrentLevel.LevelName == "Test") ? 0 : 1;
		
	}
	public void SetCampaign(bool isOn) => Campaign = isOn;
	public void SetLives(int lives) => CurrLives = lives;
	public void SetTank(TankType tank) => Tank = tank;
	public void LoadLevel(LevelInfo level) {
		CurrentLevel = level;
		SceneController.Instance.LoadScene(level.LevelName);
		Debug.Log(level.LevelName);
	}
	public void ReloadLevel() {
		SceneController.Instance.LoadScene(CurrentLevel.LevelName);
	}
	public void LoadNextLevel() {
		CurrentLevel = CurrentLevel.NextScene;
		SceneController.Instance.LoadScene(CurrentLevel.LevelName);
	}
	public object SaveState() {
		return new SaveData() {
			Kills = this.Kills,
			Deaths = this.Deaths,
			Campaign = this.Campaign
		};
	}
	public void LoadState(object state) {
		var saveData = (SaveData)state;
		Kills = saveData.Kills;
		Deaths = saveData.Deaths;
		Campaign = saveData.Campaign;
	}
	private struct SaveData {
		public int Kills;
		public int Deaths;
		public bool Campaign;
		public int CurrLives;
	}
}
public enum GameMode {
	Menu = 0,
	Campaign = 1,
	Freeplay = 2,
	Tutorial = 3,
	LevelSelect = 4,
	Continue = 5
}
[System.Serializable]
public enum GameState {
	Starting = 0,
	SpawningPlayer = 1,
	SpawningEnemies = 2,
	Play = 3,
	Win = 4,
	Dead = 5
}