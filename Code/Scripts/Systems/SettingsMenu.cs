using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

namespace strids {
	public class SettingsMenu : MonoBehaviour {
		[SerializeField] private AudioMixer _audioMixer;
		[SerializeField] private TMP_Dropdown _resolutionDropdown;
		readonly private List<Resolution> _resolutions = new();
		int currentResolution = 0;
		private int _currentMonitor;
		readonly private List<int> _monitors = new();
		public TMP_Text _monitorText;
		private void Start () {
			SetFullscreen(false);
			for (int i = 0; i < Display.displays.Length; i++) {
				Display.displays[i].Activate();
				_monitors.Add(i);
			}
			Resolution[] resolutions = Screen.resolutions;
			List<string> options = new();
			_resolutionDropdown.ClearOptions();
			int refresh = Screen.currentResolution.refreshRate;
			for (int i = 0; i < resolutions.Length; i++) {
				if (resolutions[i].refreshRate != refresh) continue;
				string option = $"{resolutions[i].width} x {resolutions[i].height}";
				_resolutions.Add(resolutions[i]);
				options.Add(option);
			}
			_resolutions.Reverse();
			options.Reverse();
			_resolutionDropdown.AddOptions(options);
		}
		public void SetMasterVolume (float volume) {
			_audioMixer.SetFloat("Volume_master", volume);
		}
		public void SetSFXVolume (float volume) {
			_audioMixer.SetFloat("Volume_sfx", volume);
		}
		public void SetMusicVolume (float volume) {
			_audioMixer.SetFloat("Volume_music", volume);
		}
		public void SetResolutionIndex (int index) {
			currentResolution = index;
			Resolution resolution = _resolutions[index];
			Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, 0);
		}
		public void SetFullscreen (bool isFullscreen) {
			Screen.fullScreen = isFullscreen;
			Screen.fullScreenMode = isFullscreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
		}
		public void ToggleDebugger (bool isOn) => Debugger.Instance.ToggleCanvas(isOn);
	}
}