using System.Collections.Generic;
using UnityEngine;

namespace strids {
	public class ProceduralCityControls : MonoBehaviour {
		public static ProceduralCityControls Instance { get; private set; }

		public List<GameObject> GeneratedObjs = new List<GameObject>();
		public PerlinGenerator PerlinGen;
		public GridSpawner GridSpawner;

		private void Awake () {
			if (Instance != null && Instance != this) {
				Destroy(gameObject);
			}
			Instance = this;
		}
		public void AddObject (GameObject obj) {
			GeneratedObjs.Add(obj);
		}
	}
}