using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace strids
{
    public class LSystemGenerator : MonoBehaviour {
        [SerializeField] private Rule[] _rules;
		[SerializeField] private string _rootSentence;
        [SerializeField, Range(0,10)] private int _iterationLimit = 1;

        [SerializeField] private bool _randomIgnoreRuleMod = true;
        [SerializeField] private float _ignoreRuleChance = 0.3f;
        [SerializeField] private bool _logs = false;
        [SerializeField] private Vector3 _transformation = Vector3.zero;

        void Start() {
            Log(GenerateSentence());
            //this.transform.position += _transformation;
        }
        public string GenerateSentence(string word = null) {
            word ??= _rootSentence;
            return GrowRecursive(word);
        }
        private string GrowRecursive(string word, int index = 0) {
            if(index >= _iterationLimit) return word;
            StringBuilder newWord = new();
            foreach (var c in word) {
                newWord.Append(c);
                ProcessRecursively(newWord, c, index);
            }
            return newWord.ToString();
        }

        private void ProcessRecursively(StringBuilder newWord, char c, int index) {
            foreach (var rule in _rules) {
                if (rule.Letter == c.ToString()) {
                    if (_randomIgnoreRuleMod && index > 1) {
                        if (Random.value < _ignoreRuleChance) return;
                    }
                    newWord.Append(GrowRecursive(rule.GetResult(), index+1));
                }
            }
        }

        private void Log(string message) {
            if (_logs) return;
            Debug.Log(message);
        }
    }
}