using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ashspace
{
    public class LSystemGenerator : MonoBehaviour {
        [SerializeField] private Rule[] _rules;
		[SerializeField] private string _rootSentence;
        [SerializeField, Range(0,10)] private int IterationLimit = 1;

        [SerializeField] private bool _randomIgnoreRuleMod = true;
        [SerializeField] private float _ignoreRuleChance = 0.3f;

        void Start() {
            Debug.Log(GenerateSentence());
        }
        public string GenerateSentence(string word = null) {
            if (word == null) word = _rootSentence;
            return GrowRecursive(word);
        }
        private string GrowRecursive(string word, int index = 0) {
            if(index >= IterationLimit) return word;
            StringBuilder newWord = new StringBuilder();
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
    }
}