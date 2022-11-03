using UnityEngine;

public class GameManager : MonoBehaviour, ISaveable {
	public static GameManager Instance { get; private set; }

	//Events
	//public static event Action<GameState> OnBeforeStateChanged;
	//public static event Action<GameState> OnAfterStateChanged;
	//public static GameState State { get; private set; }

	public TankType _tank = null;
	public int _kills = 0;
	public int _deaths = int.MaxValue;
	public bool _campaign = false;
	public int _currLives = 5;

	public GameMode _mode = GameMode.Menu;
	public int _nextScene = 0;
	public bool _paused = false;

	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
	}
	public void SetLives(int lives) => _currLives = lives;
	public void SetTank(TankType tank) => _tank = tank;
	public void SetGameMode(int mode) {
		_mode = (GameMode) mode;
		switch (_mode) {
			case GameMode.Menu:
				_nextScene = 0;
				_currLives = 0;
				break;
			case GameMode.LevelSelect:
				_nextScene = 5;
				_currLives = 4;
				break;
			case GameMode.Campaign:
				_nextScene = 5;
				_currLives = 4;
				break;
			case GameMode.Freeplay:
				_nextScene = 2;
				_currLives = 4;
				break;
			case GameMode.Tutorial:
				_nextScene = 8;
				_currLives = 5;
				break;
			case GameMode.Continue:
				//Load Game
				break;
		}
	}
	//public void Start() => ChangeState(GameState.Starting);
	//public void ChangeState(GameState newState) {
	//	OnBeforeStateChanged?.Invoke(newState);
	//	State = newState;
	//	switch (newState) {
	//		case GameState.Starting:
	//			HandleStarting();
	//			break;
	//		case GameState.SpawningPlayer:
	//			HandleStarting();
	//			break;
	//		case GameState.SpawningEnemies:
	//			HandleStarting();
	//			break;
	//		case GameState.Win:
	//			HandleStarting();
	//			break;
	//		case GameState.Dead:
	//			HandleStarting();
	//			break;
	//	}
	//	OnAfterStateChanged?.Invoke(newState);
	//	Debug.Log($"New state: {newState}");
	//}
	//private void HandleStarting() {
	//	ChangeState(GameState.SpawningPlayer);
	//}
	//private void HandleSpawningPlayer() {
	//	ChangeState(GameState.SpawningEnemies);
	//}
	//private void HandleSpawningEnemy() {
	//	ChangeState(GameState.Play);
	//}
	//private void HandlePlayMode() {

	//}
	public object SaveState() {
		return new SaveData() {
			kills = this._kills,
			deaths = this._deaths,
			campaign = this._campaign
		};
	}
	public void LoadState(object state) {
		var saveData = (SaveData)state;
		_kills = saveData.kills;
		_deaths = saveData.deaths;
		_campaign = saveData.campaign;
	}
	private struct SaveData {
		public int kills;
		public int deaths;
		public bool campaign;
		public int currLives;
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