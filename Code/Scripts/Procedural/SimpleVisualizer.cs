using System;
using System.Collections.Generic;
using UnityEngine;

namespace strids
{
    public class SimpleVisualizer : MonoBehaviour {
		[SerializeField] private LSystemGenerator _LSystem;
        private List<Vector3> _positions = new();
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Material _lineMaterial;

		[SerializeField] private int _length = 16;
		[SerializeField] private int _angle = 90;

        public int Length {
            get {
                if (_length > 0) return _length;
                else return 2;
            }
            set => _length = value;
        }
        void Start() {
            var sequence = _LSystem.GenerateSentence();
            VisualizeSequence(sequence);
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
                        DrawLine(tempPosition, currentPosition, Color.blue);
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
            foreach (var pos in _positions){
                Instantiate(_prefab, pos, Quaternion.identity, transform);
            }
        }

        private void DrawLine(Vector3 start, Vector3 end, Color color) {
            GameObject line = new("Line");
            line.transform.position = start;
            LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
            lineRenderer.material = _lineMaterial;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
            line.transform.parent = transform;
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