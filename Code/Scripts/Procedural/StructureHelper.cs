using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ashspace {
    public class StructureHelper : MonoBehaviour {
        [SerializeField] private StructureType[] _structures;
        [SerializeField] private GameObject[] _extras;
        readonly Dictionary<Vector3Int, GameObject> _strctDictionary = new();
        readonly Dictionary<Vector3Int, GameObject> _extrasDictionary = new();
        [SerializeField] private bool _randomExtras = false;
        [SerializeField, Range(0,1)] private float _extrasPlacementThreshold = 0.3f;

        public void ScaleUp(float num) => transform.localScale *= num;
        public void PlaceStructures(List<Vector3Int> roadPositions) {
            Dictionary<Vector3Int, Direction> freeSpots = FindFreeSpots(roadPositions);
            List<Vector3Int> blockedPos = new();
            foreach (var spot in freeSpots) {
                if (blockedPos.Contains(spot.Key)) continue;
                Quaternion rotation = Quaternion.identity;
                switch (spot.Value) {
                    case Direction.Up:
                        rotation = Quaternion.Euler(0, 90, 0);
                        break;
                    case Direction.Down:
                        rotation = Quaternion.Euler(0, -90, 0);
                        break;
                    case Direction.Right:
                        rotation = Quaternion.Euler(0, 180, 0);
                        break;
                    default:
                        break;
                }
                for (int i = 0; i < _structures.Length; i++) {
                    if (_structures[i].quantity == -1) {
                        if (_randomExtras) {
                            var rand = Random.value;
                            if (rand < _extrasPlacementThreshold) {
                                GameObject extraPrefab = SpawnPrefab(_extras[Random.Range(0, _extras.Length)], spot.Key, rotation, _structures[i].Parent);
                                _extrasDictionary.Add(spot.Key, extraPrefab);
                                break;
                            }
                        }
                        GameObject structure = SpawnPrefab(_structures[i].GetPrefab(), spot.Key, rotation, _structures[i].Parent);
                        _strctDictionary.Add(spot.Key, structure);
                        break;
                    }
                    if (_structures[i].IsStructureAvalable()) {
                        if (_structures[i].SizeRequired > 1) {
                            var halfSize = Mathf.CeilToInt(_structures[i].SizeRequired / 2f);
                            List<Vector3Int> tempBlockedPos = new();
                            if (VerifyIfFits(halfSize, freeSpots, spot, ref tempBlockedPos)) {
                                blockedPos.AddRange(tempBlockedPos);
                                GameObject structure = SpawnPrefab(_structures[i].GetPrefab(), spot.Key, rotation, _structures[i].Parent);
                                _strctDictionary.Add(spot.Key, structure);
                                foreach (var pos in tempBlockedPos) {
                                    if (!_strctDictionary.ContainsKey(pos)) _strctDictionary.Add(pos, structure);
                                }
                                break;
                            }
                        }
                        else {
                            GameObject structure = SpawnPrefab(_structures[i].GetPrefab(), spot.Key, rotation, _structures[i].Parent);
                            _strctDictionary.Add(spot.Key, structure);
                        }
                        break;
                    }
                }
            }
        }
        private bool VerifyIfFits(int halfSize, Dictionary<Vector3Int, Direction> freeSpots,
            KeyValuePair<Vector3Int, Direction> spot, ref List<Vector3Int> tempBlockedPos) {

            Vector3Int direction;
            if (spot.Value == Direction.Down || spot.Value == Direction.Up) {
                direction = Vector3Int.right;
            }
            else {
                direction = new(0, 0, 1);
            }
            for (int i = 1; i < halfSize; i++) {
                var pos1 = spot.Key + direction * i;
                var pos2 = spot.Key - direction * i;
                if (!freeSpots.ContainsKey(pos1) || !freeSpots.ContainsKey(pos2) ||
                    tempBlockedPos.Contains(pos1) || tempBlockedPos.Contains(pos2)) {
                    return false;
                }
                tempBlockedPos.Add(pos1);
                tempBlockedPos.Add(pos2);
            }
            return true;
        }
        private GameObject SpawnPrefab(GameObject prefab, Vector3Int pos, Quaternion rotation, Transform parent = null) {
            var newStructure = Instantiate(prefab, pos, rotation, parent);
            return newStructure;
        }
        public Dictionary<Vector3Int, Direction> FindFreeSpots(List<Vector3Int> roadPositions) {
            Dictionary<Vector3Int, Direction> freeSpots = new();
            foreach (var pos in roadPositions) {
                List<Direction> neighboutDir = PlacementHelper.FindNeighbour(pos, roadPositions);
                Quaternion rotation = Quaternion.identity;
                foreach (Direction direction in Enum.GetValues(typeof(Direction))) {
                    if (neighboutDir.Contains(direction) == false) {
                        var newPosition = pos + PlacementHelper.GetOffsetFromDirection(direction);
                        if (freeSpots.ContainsKey(newPosition)) continue;
                        freeSpots.Add(newPosition, PlacementHelper.GetReverseDirection(direction));
                    }
                }
            }
            return freeSpots;
        }
    }
}