using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTank : TankBase {
	[Header("Enemy AI")]
	[SerializeField] private NavMeshAgent _navMeshAgent;
	[SerializeField] private float _startWaitTime = 4f;
	[SerializeField] private float _timeToRotate = 2f;

	[SerializeField] private float _viewAngle = 90f;
	[SerializeField] private LayerMask _playerMask;
	[SerializeField] private LayerMask _obstacleMask;

	[SerializeField] private List<Transform> _waypoints;
	private int _currentWaypointIndex;

	Vector3 playerLastPos = Vector3.zero;
	Vector3 _playerPos;
	[SerializeField] private Transform _target;

	[Header("Modes")]
	[SerializeField] private float _waitTime;
	[SerializeField] private float _rotateTime;
	[SerializeField] private bool _PlayerInRange;
	[SerializeField] private bool _PlayerNear;
	[SerializeField] private bool _IsPatrol;
	[SerializeField] private bool _PlayerDestroyed;
	/*
	 ACTIONS
	 * patrol 
	 * chase rad = detect dist * 1.5
	 * aim rad = detectdist
	 */
	protected override void Start() {
		base.Start();
		_logger = transform.parent.GetComponent<Logger>();
		_navMeshAgent = GetComponent<NavMeshAgent>();
		_playerPos = Vector3.zero;
		_waitTime = _startWaitTime;
		_rotateTime = _timeToRotate;
		_PlayerInRange = false;
		_PlayerNear = false;
		_IsPatrol = true;
		_PlayerDestroyed = false;

		_currentWaypointIndex = 0;
		int numOfWaypoints = Random.Range(1, 5);
		GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
		for (int i = 0; i < numOfWaypoints; i++) {
			_waypoints.Add(allWaypoints[Random.Range(0, allWaypoints.Length)].transform);
		}

		_navMeshAgent.isStopped = false;
		_navMeshAgent.speed = _tank.MoveSpeed;
		_navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].position);
	}
	protected override void Update() {
		base.Update();
		EnvironmentView();
		if (!_IsPatrol) { Chasing(); }
		else { Patroling(); }
	}
	private void FixedUpdate() {
		if (_firePoint != null) {
			if (_PlayerInRange && _nextFire <= 0f) {
				_nextFire = _tank.FireRate;
				Shoot();
			}
		}
	}
	protected virtual void Aim(Vector3 target) {
		if (!_canRotate) return;
		Vector3 relativePos = target - _turret.position;
		Quaternion toRotation = Quaternion.LookRotation(relativePos);
		_turret.rotation = Quaternion.Lerp(_turret.rotation, toRotation, _tank.TurretRotSpeed * Time.deltaTime);
	}
	void MoveToWaypoint() {
		_navMeshAgent.isStopped = false;
		_navMeshAgent.speed = _tank.MoveSpeed;
	}
	void Stop() {
		_navMeshAgent.isStopped = true;
		_navMeshAgent.ResetPath();
		_navMeshAgent.speed = _tank.MoveSpeed;
	}
	protected override void OnDead() {
		base.OnDead();
		Destroy(gameObject);
	}
	void Patroling() {
		_navMeshAgent.stoppingDistance = Mathf.Clamp(_tank.DetectDist/3f, 1.6f, float.MaxValue);
		if (_PlayerNear) {
			_logger.Log("Patrol >> Search", this);
			if (_rotateTime <= 0) {
				MoveToWaypoint();
				LookingForPlayer(playerLastPos);
			}
			else {
				_logger.Log("Finished Searching", this);
				Stop();
				_rotateTime -= Time.deltaTime;
			}
		}
		else {
			_logger.Log("Patroling", this);
			_PlayerNear = false;
			playerLastPos = Vector3.zero;
			_navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].position);
			if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance) {
				if (_waitTime <= 0) {
					NextWaypoint();
					MoveToWaypoint();
					_waitTime = _startWaitTime;
				}
				else {
					Stop();
					_waitTime -= Time.deltaTime;
				}
			}
		}
	}
	void Chasing() {
		_PlayerNear = false;
		_navMeshAgent.stoppingDistance = Mathf.Clamp(_tank.DetectDist / 2f, 1, float.MaxValue);
		playerLastPos = Vector3.zero;
		if (!_PlayerDestroyed) {
			MoveToWaypoint();
			_navMeshAgent.SetDestination(_playerPos);
			Aim(_playerPos);
			_logger.Log("Chasing", this);
		}
		if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance) {
			if (_waitTime <= 0 && !_PlayerDestroyed && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f) {
				_logger.Log("Chasing >> Patrol", this);
				_IsPatrol = true;
				_PlayerNear = false;
				MoveToWaypoint();
				_rotateTime = _timeToRotate;
				_waitTime = _startWaitTime;
				_navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].position);
				Aim(_target.position);
			}
			else {
				if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f) {
					Stop();
				}
				_logger.Log("Chasing Finished", this);
				_waitTime -= Time.deltaTime;
				if (_waitTime <= 0) {
					_logger.Log("Chasing >> Patrol", this);
					_IsPatrol = true;
					_PlayerNear = false;
					_rotateTime = _timeToRotate;
					_waitTime = _startWaitTime;
					_navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].position);
					Aim(_target.position);
				}
			}
		}
	}
	void LookingForPlayer(Vector3 player) {
		_logger.Log("Searching", this);
		_navMeshAgent.SetDestination(player);
		if (Vector3.Distance(transform.position, player) <= 0.3f) {
			if (_waitTime <= 0) {
				_logger.Log("Search >> Patrol", this);
				_PlayerNear = false;
				MoveToWaypoint();
				_navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].position);
				_rotateTime = _timeToRotate;
				_waitTime = _startWaitTime;
			}
			else {
				Stop();
				_waitTime -= Time.deltaTime;
			}
		}
	}
	public void NextWaypoint() {
		_currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Count;
		_navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].position);
	}
	void PlayerDestroyed() => _PlayerDestroyed = true;
	void EnvironmentView() {
		Collider[] playerInRanage = Physics.OverlapSphere(transform.position, _tank.DetectDist, _playerMask);

		for (int i = 0; i < playerInRanage.Length; i++) {
			Transform player = playerInRanage[i].transform;
			Vector3 dirToPlayer = (player.position - transform.position).normalized;
			if (Vector3.Angle(transform.forward, dirToPlayer) < _viewAngle / 2) {
				float dstToplayer = Vector3.Distance(transform.position, player.position);
				if (!Physics.Raycast(transform.position, dirToPlayer, dstToplayer, _obstacleMask)) {
					_PlayerInRange = true;
					_IsPatrol = false;
				}
				else { _PlayerInRange = false; }
			}
			if (Vector3.Distance(transform.position, player.position) > _tank.DetectDist) _PlayerInRange = false;
			if (_PlayerInRange) _playerPos = player.transform.position;
		}
	}
	protected override void OnDrawGizmos() {
		if (_showGizmos) { 
			base.OnDrawGizmos();
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(_navMeshAgent.destination, 2f);
		}
	}
}