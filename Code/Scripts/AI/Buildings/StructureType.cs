using System;
using UnityEngine;

namespace strids {
    [Serializable]
    public class StructureType {
        [SerializeField] private GameObject[] _prefabs;
        public Transform Parent;
        public int SizeRequired;
        public int quantity;
        public int quantityPlaced;

        public GameObject GetPrefab() {
            quantityPlaced++;
            if (_prefabs.Length > 1) {
                var random = UnityEngine.Random.Range(0, _prefabs.Length);
                return _prefabs[random];
            }
            return _prefabs[0];
        }
        public bool IsStructureAvalable() => quantityPlaced < quantity;
        public void Reset() => quantityPlaced = 0;
    }
}