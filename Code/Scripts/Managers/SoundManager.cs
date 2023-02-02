using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance { get; private set; }
	[SerializeField] private AudioSource _musicSource;
	[SerializeField] private AudioSource _soundSource;
	private void Awake() {
		if (Instance != null) { Destroy(this); }
		else { Instance = this; }
	}
	public void PlayMusic(AudioClip clip) {
		_musicSource.clip = clip;
		_musicSource.Play();
	}
	public void PlaySound(AudioClip clip, Vector3 pos, float vol = 1) {
		_soundSource.clip = clip;
		PlaySound(clip, vol);
	}
	public void PlaySound(AudioClip clip, float vol = 1) {
		_soundSource.PlayOneShot(clip, vol);
	}
	public void ChangeVolume(float value) {
		AudioListener.volume = value;
	}
	public void ToggleMusic() {
		_musicSource.mute = !_musicSource.mute;
	}
	public void ToggleSFX() {
		_soundSource.mute = !_soundSource.mute;
	}
}