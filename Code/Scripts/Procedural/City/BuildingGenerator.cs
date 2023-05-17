using UnityEngine;

namespace strids {
    public class BuildingGenerator : MonoBehaviour {
		public int MinPieces = 20;
		public int MaxPieces = 20;
		public float PerlineScaleFactor = 2f;

		public int RandomVariationMin = -5;
		public int RandomVariationMax = 10;
		public GameObject[] baseParts;
		public GameObject[] middleParts;
		public GameObject[] topParts;

		void Start () {
			Build();
		}
		public void Build () {
			float sampledVal = PerlinGenerator.Instance.PerlinSteppedPos(transform.position);
			int targetPieces = Mathf.FloorToInt(MaxPieces * sampledVal);
			targetPieces += Random.Range(RandomVariationMin, RandomVariationMax);
			if (targetPieces <= 0) return;
		}
	}
}