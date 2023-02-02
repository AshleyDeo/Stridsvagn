using UnityEngine;

namespace ashspace {
	[CreateAssetMenu(fileName = "New Rule", menuName = "Assets/Rule")]
	public class Rule : ScriptableObject {
		public string Letter;
		[SerializeField] private string[] results = null;
		[SerializeField] private bool randomResults = false;

		public string GetResult() {
			if (randomResults) {
				int randomIndex = Random.Range(0,results.Length);
				return results[randomIndex];
			}
			return results[0];
		}
    }
}