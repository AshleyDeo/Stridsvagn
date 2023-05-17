using UnityEngine;

namespace strids {
    public class BuildingGeneratorNoiseInput : MonoBehaviour {
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
            float sampledVal = PerlinGenerator.Instance.PerlinSteppedPos(this.transform.position);
            int targetPieces =  Mathf.FloorToInt(MaxPieces * sampledVal);
            targetPieces += Random.Range(RandomVariationMin, RandomVariationMax);
            if (targetPieces <= 0) return;

            float heightOffset = SpawnPieceLayer(baseParts, 0);

            for (int i = 2; i < targetPieces; i++) {
                heightOffset += SpawnPieceLayer(middleParts, heightOffset);
            }
            SpawnPieceLayer(topParts, heightOffset);
        }

        private float SpawnPieceLayer (GameObject[] objs, float offset) {
            Transform randTransform = objs[Random.Range(0, objs.Length)].transform;
            GameObject clone = Instantiate(randTransform.gameObject, this.transform.position + new Vector3(0, offset, 0), Quaternion.identity);
            Mesh cloneMmesh = clone.GetComponent<MeshFilter>().mesh;
            Bounds baseBounds = cloneMmesh.bounds;
            float heightOffset = baseBounds.size.y;

            clone.transform.SetParent(this.transform);
            ProceduralCityControls.Instance.AddObject(clone);

            return heightOffset;
        }
	}
}