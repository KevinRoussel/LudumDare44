using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour {

    protected GameObject _player;

    [Header("Movement variables")]
    [Tooltip("Maximum distance for the enemy to chase the player")]
    [SerializeField] protected float _playerDetectionDistance;
    protected bool _playerDetected;

    [Tooltip("Start of patrol path")]
    [SerializeField] protected Transform _patrolPathStart;
    [Tooltip("End of patrol path")]
    [SerializeField] protected Transform _patrolPathEnd;
    protected bool _movingToEnd = true;

    protected NavMeshAgent _navMeshAgent;    

    public virtual void Initialization () {

        _player = GameObject.FindGameObjectWithTag("Player");
        _navMeshAgent = GetComponent<NavMeshAgent>();
        SetDestination();        

    }

    public void Tick () {

        if (Vector3.Distance(_player.transform.position, transform.position) <= _playerDetectionDistance) {

            if(!_playerDetected)
                PlayerDetected();

        }  else {

            if (_playerDetected)
                PlayerLost();

            Movement();

        }

    }

    protected void SetDestination () {

        NavMesh.SamplePosition((_movingToEnd ? _patrolPathEnd : _patrolPathStart).position, out NavMeshHit hit, 1f, NavMesh.AllAreas);
        _navMeshAgent.SetDestination(hit.position);

    }

    protected virtual void PlayerDetected () {

        _playerDetected = true;

    }    

    protected virtual void PlayerLost () {

        _playerDetected = false;

    }

    protected virtual void Movement () {

        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance) {

            _movingToEnd ^= true;
            SetDestination();

        }

    }

}
