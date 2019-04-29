using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemy : MonoBehaviour {

    protected GameObject _player;

    [Header("Movement variables")]
    [Tooltip("Maximum distance for the enemy to chase the player")]
    [SerializeField] protected float _playerDetectionDistance;
    protected bool _playerDetected;

    protected Vector3 _patrolPathStart, _patrolPathEnd;
    [Tooltip("End of patrol path")]
    [SerializeField] protected Transform _patrolPathEndPos;
    protected bool _movingToEnd = true;

    protected NavMeshAgent _navMeshAgent;    
    protected Character _character;


    public int CanMove { get; set; }

    void Awake() {
        _character = GetComponent<Character>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.enabled = false;

        // TODO

    }

    public virtual void Initialization () {

        _player = GameObject.FindGameObjectWithTag("Player");

        _patrolPathStart = transform.position;
        _patrolPathEnd = _patrolPathEndPos.transform.position;

        _navMeshAgent.enabled = true;
        SetDestination();        
    }

    public virtual void Tick () {

        if (Vector3.Distance(_player.transform.position, transform.position) <= _playerDetectionDistance) {

            PlayerDetected();

        }  else {

            if (_playerDetected)
                PlayerLost();

            Movement();

        }
    }

    protected void SetDestination () {

        _character.FireWalk();
        SetNavDestination(_movingToEnd ? _patrolPathEnd : _patrolPathStart);

    }

    protected void SetNavDestination (Vector3 target) {

        NavMesh.SamplePosition(target, out NavMeshHit hit, 10f, NavMesh.AllAreas);
        if(_navMeshAgent.enabled)
            _navMeshAgent.SetDestination(hit.position);

    }

    protected virtual void PlayerDetected () {

        _playerDetected = true;

    }    

    protected virtual void PlayerLost () {

        _playerDetected = false;

    }

    Coroutine c;
    protected virtual void Movement () {

        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance) {
            _movingToEnd ^= true;
            _character.FireStopWalk();

            if (c == null)
            {
                c = StartCoroutine(Extension.WaitSecondsAnd(0.5f, () => { SetDestination(); c = null; }));
            }

        }

    }

    public virtual void Flashed (float duration, Vector2 upgradeEffect) {

        GetComponent<Character>()?.Flashed();
        SetCanMove(false);

        StartCoroutine(Extension.WaitSecondsAnd(duration, () => {

            SetCanMove(true);

            if(upgradeEffect != Vector2.zero) {

                _navMeshAgent.speed /= upgradeEffect.x;

                StartCoroutine(Extension.WaitSecondsAnd(upgradeEffect.y, () => { _navMeshAgent.speed *= upgradeEffect.x; }));

            }

        }));

    }

    public void SetCanMove(bool can) {

        CanMove += (can ? 1 : -1);
        _navMeshAgent.isStopped = (CanMove < 0);
    }

}
