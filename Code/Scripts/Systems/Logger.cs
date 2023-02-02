using UnityEngine;

public class Logger : MonoBehaviour {
	[Header("Logs")]
	[SerializeField] private bool _showLogs;
	[SerializeField] private string _prefix;
	[SerializeField] private Color _color;
	string _hexColor;

	[Header("Objects")]
	public bool showDebugObjs = true;
	[SerializeField] private GameObject[] _debugObjects;

	void OnValidate() {
		_hexColor = "#"+ColorUtility.ToHtmlStringRGB(_color);
	}
	public void Log(object message, Object sender) {
		if (!_showLogs) return;
		Debug.Log($"<color={_hexColor}>{_prefix}:</color> {sender.name} - {message}", sender);
	}
	public void ToggleDebugObjs() {
		showDebugObjs = !showDebugObjs;
		//DebugObjs();
	}
	public void DebugObjs(bool show) {
		for (int i = 0; i < _debugObjects.Length; i++) {
			_debugObjects[i].SetActive(show);
		}
	}
}