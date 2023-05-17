using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace strids
{
    public class LoadScreenManager : MonoBehaviour {
        [Header("General Info")]
        [SerializeField] private TMP_Text _location;
        [SerializeField] private TMP_Text _battery;
        [SerializeField] private Image _progressBar;
        [SerializeField] private Transform _lives;

        [Header("Tank")]
        [SerializeField] private Transform _tank;
        [SerializeField] private TankType _defaultTank;
        [SerializeField] private Transform _health;

		[Header("Debug")]
        [SerializeField, Range(0f, 0.5f)] private float _testFloat;

		private GameManager _gm;
        void OnEnable() {
            _gm = GameManager.Instance;
            ResetTank();
            SetProgress();
        }
        void OnValidate() {
			//_progressBar.material.SetFloat("_Progress", _testFloat);
		}
        void ResetTank() {
			foreach (Transform child in _tank) {
				GameObject.Destroy(child.gameObject);
			}
            _tank.localScale = Vector3.one;
            TankType tank = _gm.Tank == null ? _defaultTank : _gm.Tank;
            Instantiate(tank.UI_Hull, _tank);
            Instantiate(tank.UI_Turret, _tank);
            _tank.localScale *= 300;
		}

        private void SetProgress() {
			_progressBar.material.SetFloat("_Progress", _gm.LevelsCompleted / (float)_gm.MaxLevels / 2f);
        }
    }
}