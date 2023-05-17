using UnityEngine;
using UnityEngine.UI;

namespace strids {
    public class PerlinGenerator : MonoBehaviour {
		public static PerlinGenerator Instance { get; private set; }

		[SerializeField] private int _perlinTextureSizeX;
		[SerializeField] private int _perlinTextureSizeY;
		[SerializeField] private bool randomizeNoiseOffset;
		[SerializeField] private int _perlinGridStepSizeX;
		[SerializeField] private int _perlinGridStepSizeY;
		[SerializeField] private Vector2 _perlinOffset;
		[SerializeField] private float noiseScale = 1f;

		[SerializeField] private bool visualizeGrid = false;
		[SerializeField] private GameObject visualizationCube;
		[SerializeField] private float visualizationHeightScale = 5f;
		[SerializeField] private GameObject visualizationUI;

		private Texture2D perlinTexture;

		private void Awake () {
			if (Instance != null && Instance != this) {
				Destroy(gameObject);
			}
			Instance = this;
		}
		void Start () {
			Generate();
		}
		public void Generate () {
			GenerateNoise();
			if (visualizeGrid) VisualizeGrid();
        }
		private void VisualizeGrid () {
			GameObject visualizationParent = new("Visualization");
			visualizationParent.transform.SetParent(this.transform);

			for (int x = 0; x < _perlinGridStepSizeX; x++) {
				for (int y = 0; y < _perlinGridStepSizeY; y++) {
					GameObject clone = Instantiate(visualizationCube, new Vector3(x, SampleStepped(x, y) * visualizationHeightScale, y) + transform.position, transform.rotation) as GameObject;

					clone.transform.SetParent(visualizationParent.transform);
					ProceduralCityControls.Instance.AddObject(clone);

				}
			}
			visualizationParent.transform.position = new Vector3(-_perlinGridStepSizeX * .5f, -visualizationHeightScale * .5f, -_perlinGridStepSizeY * .5f);
		}
		private void GenerateNoise () {
			if (randomizeNoiseOffset) {
				_perlinOffset = new Vector2(Random.Range(0, 99999), Random.Range(0, 99999));
			}

			perlinTexture = new Texture2D(_perlinTextureSizeX, _perlinTextureSizeY);

			for (int x = 0; x < _perlinTextureSizeX; x++) {
				for (int y = 0; y < _perlinTextureSizeY; y++) {
					perlinTexture.SetPixel(x,y, SampleNoise(x,y));
				}
			}
		}
		public float PerlinSteppedPos(Vector3 pos) {
			int xToSample = Mathf.FloorToInt(pos.x + _perlinGridStepSizeX * .5f);
			int yToSample = Mathf.FloorToInt(pos.y + _perlinGridStepSizeY * .5f);
			xToSample %= _perlinGridStepSizeX;
			yToSample %= _perlinGridStepSizeY;
			return SampleStepped(xToSample, yToSample);
		}
		public float SampleStepped(int x, int y) {
			int gridStepSizeX = _perlinTextureSizeX / _perlinGridStepSizeX;
			int gridStepSizeY = _perlinTextureSizeY / _perlinGridStepSizeY;

			float sample = perlinTexture.GetPixel(Mathf.FloorToInt(x * gridStepSizeX), Mathf.FloorToInt(y * gridStepSizeY)).grayscale;
			return sample;
		}
		public void SetNoiseScaleFromSlider(Slider slider) {
			noiseScale = slider.value;
		}
		Color SampleNoise(int x, int y) {
			float xPos = (float)x / _perlinTextureSizeX * noiseScale + _perlinOffset.x;
			float yPos = (float)y / _perlinTextureSizeY * noiseScale + _perlinOffset.y;

			float sample = Mathf.PerlinNoise(xPos, yPos);
			return new Color(sample,sample,sample);
		}
    }
}