using UnityEngine;

namespace strids
{
    public class Waypoint : MonoBehaviour {
        [SerializeField] protected float _radius = 1f;
		[Header("Debug")]
		[SerializeField] protected bool _showGizmos;
		[SerializeField] protected Color _gizmoColor = Color.cyan;
		protected void Update() {
			Collider[] colliders = Physics.OverlapSphere(transform.position, _radius+0.5f, 8);
			if (colliders.Length > 0) { Destroy(gameObject); }
		}
		protected virtual void OnDrawGizmos() {
			if (_showGizmos) {
				//Detection Range
				Gizmos.color = _gizmoColor;
				Gizmos.DrawWireSphere(transform.position, _radius);
			}
		}
    }
}