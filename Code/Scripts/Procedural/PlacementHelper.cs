using System;
using System.Collections.Generic;
using UnityEngine;

namespace strids {
    public static class PlacementHelper {
        public static List<Direction> FindNeighbour(Vector3Int pos, ICollection<Vector3Int> collection) {
            List<Direction> neigbourDir = new();
            if (collection.Contains(pos + Vector3Int.forward)) {
                neigbourDir.Add(Direction.Up);
            }
            if (collection.Contains(pos + Vector3Int.back)) {
                neigbourDir.Add(Direction.Down);
            }
            if (collection.Contains(pos + Vector3Int.left)) {
                neigbourDir.Add(Direction.Left);
            }
            if (collection.Contains(pos + Vector3Int.right)) {
                neigbourDir.Add(Direction.Right);
            }
            return neigbourDir;
        }

		internal static Vector3Int GetOffsetFromDirection(Direction direction) {
            switch (direction) {
                case Direction.Up:
                    return new Vector3Int(0, 0, 1);
                case Direction.Down:
                    return new Vector3Int(0, 0, -1);
                case Direction.Left:
                    return new Vector3Int(-1, 0, 0);
                case Direction.Right:
					return new Vector3Int(1, 0, 0);
                default:
                    break;
			}
            throw new Exception($"{direction} does not exist!");
		}

        public static Direction GetReverseDirection(Direction direction) {
			switch (direction) {
				case Direction.Up:
					return Direction.Down;
				case Direction.Down:
					return Direction.Up;
				case Direction.Left:
					return Direction.Right;
				case Direction.Right:
					return Direction.Left;
				default:
					break;
			}
			throw new Exception($"{direction} does not exist!");
		}
    }
}