using System;
using System.Collections.Generic;
using UnityEngine;

namespace ashspace
{
    public class Visualizer : MonoBehaviour {
		[SerializeField] private LSystemGenerator _LSystem;
		[SerializeField] private RoadHelper _roadHelper;
		[SerializeField] private StructureHelper _structHelper;

        private readonly List<Vector3> _positions = new();

		[SerializeField] private int _scale = 3;
		[SerializeField] private int _length = 8;
		[SerializeField] private int _angle = 90;
        [Header("Debug")]
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Material _lineMaterial;

        public int Length {
            get {
                if (_length > 0) return _length;
                else return 1;
            }
            set => _length = value;
        }
        void Start() {
            var sequence = _LSystem.GenerateSentence();
            VisualizeSequence(sequence);
            transform.localScale = new(_scale, 1, _scale);
        }
        private void VisualizeSequence(string sequence) {
            Stack<AgentParameters> savePoints = new();
            Vector3 currentPosition = Vector3.zero;
            Vector3 direction = Vector3.forward;
            _positions.Add(currentPosition);
            foreach(var letter in sequence){
                EncodingLetters encoding = (EncodingLetters)letter;
                switch (encoding) {
                    case EncodingLetters.save:
                        savePoints.Push(new AgentParameters {
							position = currentPosition,
							direction = direction,
                            length = Length
                        });
                        break;
                    case EncodingLetters.load:
                        if (savePoints.Count > 0) {
                            AgentParameters agentParamters = savePoints.Pop();
                            currentPosition = agentParamters.position;
                            direction = agentParamters.direction;
                            Length = agentParamters.length;
                        }
                        else {
                            throw new System.Exception("Dont have save point in stack");
                        }
                        break;
                    case EncodingLetters.draw:
                        Vector3 tempPosition = currentPosition;
                        currentPosition += direction * Length;
                        _roadHelper.PlaceStreetPositions(tempPosition, Vector3Int.RoundToInt(direction), Length);
                        Length -= 2;
                        _positions.Add(currentPosition);
                        break;
                    case EncodingLetters.turnRight:
                        direction = Quaternion.AngleAxis(_angle, Vector3.up) * direction;
                        break;
                    case EncodingLetters.turnLeft:
						direction = Quaternion.AngleAxis(-_angle, Vector3.up) * direction;
						break;
                    default:
                        break;
                } 
            }
            //foreach (var pos in _positions){
            //    Instantiate(_prefab, pos, Quaternion.identity, transform);
            //}
            _roadHelper.FixRoad();
            _structHelper.PlaceStructures(_roadHelper.GetRoadPositions());
        }

        public enum EncodingLetters {
            unknown = ' ',
            save = '[',
            load = ']',
            draw = 'F',
            turnRight = '+',
            turnLeft = '-'
        }
    }
}