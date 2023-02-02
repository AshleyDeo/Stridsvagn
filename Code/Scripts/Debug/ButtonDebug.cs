using UnityEngine;
using TMPro;

namespace ashspace
{
    public class ButtonDebug : MonoBehaviour {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private string _type;
        private void OnValidate() {
            if (_text != null) {
                gameObject.name = $"{_text.text} {_type}";
            }
        }
    }
}