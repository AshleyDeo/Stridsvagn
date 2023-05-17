using UnityEngine;

namespace strids {
    public class GridSpawner : MonoBehaviour {
		public int GridX, GridZ;
		[SerializeField] private GameObject prefab;
		[SerializeField] private Vector3 gridOrigin;
		public int GridOffset;
		[SerializeField] private bool GenerateOnEnable;

		public void Generate () {

		}
	}
}