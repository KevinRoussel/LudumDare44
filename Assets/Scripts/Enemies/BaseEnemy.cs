using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour {

    EnemiesManager _enemiesManager;

    [Header("Navigation variables")]
    [Tooltip("Maximum distance for the enemy to chase the player")]
    [SerializeField] float _playerDetectionDistance;

    [Tooltip("Range for the distance of the next wandering position from current position")]
    [SerializeField] Vector2 _nextWanderingPosRange;

    NavMeshAgent _navMeshAgent;

    GameObject _player;

    void Start () {

        _enemiesManager = FindObjectOfType<EnemiesManager>();

        _player = GameObject.FindGameObjectWithTag("Player");

        _navMeshAgent = GetComponent<NavMeshAgent>();

        _enemiesManager.Enemies.Add(this);

    }

    public void Movement () {

        if (Vector3.Distance(_player.transform.position, transform.position) <= _playerDetectionDistance)
            _navMeshAgent.SetDestination(_player.transform.position);
        else if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance) {

            Vector2 randomPosOnCircle = Random.insideUnitCircle;

            Vector3 randomPointInRange = new Vector3(randomPosOnCircle.x, 0, randomPosOnCircle.y) * Random.Range(_nextWanderingPosRange.x, _nextWanderingPosRange.y);

            NavMesh.SamplePosition(transform.position + randomPointInRange, out NavMeshHit hit, _nextWanderingPosRange.y, NavMesh.AllAreas);

            _navMeshAgent.SetDestination(hit.position);

        }

    }

}
