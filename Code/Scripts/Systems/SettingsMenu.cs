using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

namespace ashspace
{
    public class SettingsMenu : MonoBehaviour {
		[SerializeField] private AudioMixer _audioMixer;
		[SerializeField] private TMP_Dropdown _resolutionDropdown;
		private Resolution[] _resolutions;
		private void Start() {
			_resolutions = Screen.resolutions;
			_resolutionDropdown.ClearOptions();
			List<string> options = new();
			for (int i = 0; i < _resolutions.Length; i++) {
				string option = $"{_resolutions[i].width} x {_resolutions[i].height} @ {_resolutions[i].refreshRate} Hz";
				options.Add(option);
			}
			_resolutionDropdown.AddOptions(options);
		}
		public void SetMasterVolume(float volume) {
			_audioMixer.SetFloat("Volume_master", volume);
		}
		public void SetSFXVolume(float volume) {
			_audioMixer.SetFloat("Volume_sfx", volume);
		}
		public void SetMusicVolume(float volume) {
			_audioMixer.SetFloat("Volume_music", volume);
		}
		public void SetResolutionIndex(int index) {
			Resolution resolution = _resolutions[index];
			Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
		}
		public void SetFullscreen(bool isFullscreen) {
			Screen.fullScreen = isFullscreen;
		}
	}
}