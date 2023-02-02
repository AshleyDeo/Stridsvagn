using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ashspace
{
    public class RoadHelper : MonoBehaviour {
        public GameObject[] _roadStraight;
        public GameObject[] _roadCorner;
        public GameObject[] _road3Way;
        public GameObject[] _road4Way;
        public GameObject[] _roadEnd;

        Dictionary<Vector3Int, GameObject> _roadDictionary = new();
        HashSet<Vector3Int> _fixRoadCandidates = new();

        public void ScaleUp(float num) => transform.localScale = new(num, 1f, num);
        public List<Vector3Int> GetRoadPositions() => _roadDictionary.Keys.ToList();
        public void PlaceStreetPositions(Vector3 startPos, Vector3Int direction, int length) {
            Quaternion rotation = Quaternion.identity;
            if (direction.x == 0) rotation = Quaternion.Euler(0, 90, 0);
            for (int i = 0; i < length; i++){
                Vector3Int pos = Vector3Int.RoundToInt(startPos + direction * i);
                if (_roadDictionary.ContainsKey(pos)) continue;
                GameObject road = Instantiate(_roadStraight[0], pos, rotation, transform);
                _roadDictionary.Add(pos, road);
                if (i == 0 || i == length - 1) {
                    _fixRoadCandidates.Add(pos);
                }
            }
        }
        public void FixRoad() {
            foreach (var pos in _fixRoadCandidates) {
                List<Direction> neighboutDir = PlacementHelper.FindNeighbour(pos, _roadDictionary.Keys);
                Quaternion rotation = Quaternion.identity;
                if (neighboutDir.Count == 1) {
					Destroy(_roadDictionary[pos]);
                    if (neighboutDir.Contains(Direction.Down)) {
                        rotation = Quaternion.Euler(0, 90, 0);
                    } else if (neighboutDir.Contains(Direction.Left)) {
						rotation = Quaternion.Euler(0, 180, 0);
					} else if (neighboutDir.Contains(Direction.Up)) {
						rotation = Quaternion.Euler(0, -90, 0);
					}
					_roadDictionary[pos] = Instantiate(_roadEnd[0], pos, rotation, transform);
				}
                else if (neighboutDir.Count == 2) {
                    if (neighboutDir.Contains(Direction.Up) && neighboutDir.Contains(Direction.Down) 
                        && neighboutDir.Contains(Direction.Left) && neighboutDir.Contains(Direction.Right)) {
                        continue;
					}
					Destroy(_roadDictionary[pos]);
                    if (neighboutDir.Contains(Direction.Right) && neighboutDir.Contains(Direction.Up)) {
                        rotation = Quaternion.Euler(0, 90, 0);
                    }
                    else if (neighboutDir.Contains(Direction.Right) && neighboutDir.Contains(Direction.Down)) {
                        rotation = Quaternion.Euler(0, 180, 0);
                    }
                    else if (neighboutDir.Contains(Direction.Down) && neighboutDir.Contains(Direction.Left)) {
                        rotation = Quaternion.Euler(0, -90, 0);
                    }
						_roadDictionary[pos] = Instantiate(_roadCorner[0], pos, rotation, transform);
				}
                else if (neighboutDir.Count == 3) {
					Destroy(_roadDictionary[pos]);
					if (neighboutDir.Contains(Direction.Right) && neighboutDir.Contains(Direction.Left) 
                        && neighboutDir.Contains(Direction.Down)) {
						rotation = Quaternion.Euler(0, 90, 0);
					}
					else if (neighboutDir.Contains(Direction.Left) && neighboutDir.Contains(Direction.Down) 
                        && neighboutDir.Contains(Direction.Up)) {
						rotation = Quaternion.Euler(0, 180, 0);
					}
					else if (neighboutDir.Contains(Direction.Left) && neighboutDir.Contains(Direction.Right) 
                        && neighboutDir.Contains(Direction.Up)) {
						rotation = Quaternion.Euler(0, -90, 0);
					}
					_roadDictionary[pos] = Instantiate(_road3Way[0], pos, rotation, transform);
				}
                else {
                    Destroy(_roadDictionary[pos]);
                    _roadDictionary[pos] = Instantiate(_road4Way[0], pos, rotation, transform);
                }
            }
        }
    }
}