using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace strids
{
    public class ButtonDebug : MonoBehaviour {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private string _type;
        private void OnValidate() {
            if (_text != null) {
                if (_text.text.Length > 0)gameObject.name = $"{_text.text} {_type}";
            }
            //Button button = GetComponent<Button>();
            //if (button != null) {
            //    _text.color = button.interactable ? Color.white : Color.grey;
            //}
        }
    }
}