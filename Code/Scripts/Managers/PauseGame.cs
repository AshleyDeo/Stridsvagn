using UnityEngine;
using UnityEngine.InputSystem;

namespace ashspace
{
    public class PauseGame : MonoBehaviour {
        [SerializeField] private InputAction _pauseButton;
        [SerializeField] private Canvas _pauseCanvas;
        private bool _paused = false;

        private void OnEnable() {
            _pauseButton.Enable();
        }
        void OnDisable() {
            _pauseButton.Disable();
        }
        void Start() {
            _pauseButton.performed += _ => Paused();
        }
        public void Paused() {
            _paused = !_paused;
            if (_paused) {
                Time.timeScale = 0;
                _pauseCanvas.enabled = true;
            }
            else {
                Time.timeScale = 1;
				_pauseCanvas.enabled = false;
			}
        }
    }
}