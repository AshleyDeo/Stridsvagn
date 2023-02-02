using System.Collections.Generic;
using UnityEngine;

namespace ashspace {
    public class ConnectedWaypoint : Waypoint {
		[Header("Connect Waypoints")]
		[SerializeField] protected float _connectRadius = 10f;
        List<ConnectedWaypoint> _connections;
		[Header("Debug")]
		[SerializeField] protected Color _gizmoColorConnect = Color.magenta;
		protected void Start() {
            GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
            _connections = new List<ConnectedWaypoint>();
            for (int i = 0; i < waypoints.Length; i++) {
                ConnectedWaypoint nextWaypoint = waypoints[i].GetComponent<ConnectedWaypoint>();
                if (nextWaypoint != null && 
                    Vector3.Distance(transform.position, nextWaypoint.transform.position) < _connectRadius) 
                    {
                        _connections.Add(nextWaypoint);
                }
            }
		}
		protected override void OnDrawGizmos() {
			if (_showGizmos) {
				base.OnDrawGizmos();
				//Detection Range
				Gizmos.color = _gizmoColorConnect;
				Gizmos.DrawWireSphere(transform.position, _connectRadius);
			}
		}
	}
}