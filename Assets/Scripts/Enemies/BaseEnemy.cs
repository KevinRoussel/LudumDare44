using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour {

    GameObject _player;

    [Header("Movement variables")]
    [Tooltip("Maximum distance for the enemy to chase the player")]
    [SerializeField] float _playerDetectionDistance;

    [Tooltip("Range for the distance of the next wandering position from current position")]
    [SerializeField] Vector2 _nextWanderingPosRange;

    [Tooltip("If the player gets too far from the enemy, should the enemy go back to wandering or walk to the last known position of the player ?")]
    [SerializeField] bool _wanderOnPlayerLost;
    bool _wasChasingPlayer;

    NavMeshAgent _navMeshAgent;

    [Header("Shooting variables")]
    [Tooltip("Shooting manager of this enemy")]
    [SerializeField] ShootingManager _shootingManager;
    [SerializeField] int _bulletPower;

    [Tooltip("Maximum distance for the enemy to shoot at the player")]
    [SerializeField] float _shootingDistance;

    [Tooltip("Range for the \"spread\" of the shooting")]
    [SerializeField] Vector2 _shootingSpreadRange;

    [Tooltip("X is the delay between stopping movement and shooting, Y is the delay between shooting and resuming movement")]
    [SerializeField] Vector2 _shootingDelays;

    [Tooltip("Cooldown between two shots fired")]
    [SerializeField] float _shootingCooldown;
    float _shootingTimer;

    [Tooltip("If true, the enemy can shoot right after it spawned, otherwise it will wait for its shooting cooldown")]
    [SerializeField] bool _canShootImmediately;

    public void Initialization () {

        _player = GameObject.FindGameObjectWithTag("Player");
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _shootingTimer = _canShootImmediately ? _shootingCooldown : 0;
        _shootingManager = GameObject.FindObjectOfType<ShootingManager>();

    }

    public void Movement () {

        _shootingTimer += (_shootingTimer != -1) ? Time.deltaTime : 0;

        if (Vector3.Distance(_player.transform.position, transform.position) <= _playerDetectionDistance) {
            _navMeshAgent.SetDestination(_player.transform.position);
            _wasChasingPlayer = true;

        }  else if ((_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance) || (_wasChasingPlayer && _wanderOnPlayerLost)) {

            _wasChasingPlayer = false;
            Vector2 randomPosOnCircle = Random.insideUnitCircle;
            Vector3 randomPointInRange = new Vector3(randomPosOnCircle.x, 0, randomPosOnCircle.y) * Random.Range(_nextWanderingPosRange.x, _nextWanderingPosRange.y);
            NavMesh.SamplePosition(transform.position + randomPointInRange, out NavMeshHit hit, _nextWanderingPosRange.y, NavMesh.AllAreas);
            _navMeshAgent.SetDestination(hit.position);
        }

        if((Vector3.Distance(_player.transform.position, transform.position) <= _shootingDistance) && 
            (_shootingTimer >= _shootingCooldown))
            StartCoroutine(Shoot());

    }

    IEnumerator Shoot () {

        _shootingTimer = -1;
        _navMeshAgent.updatePosition = false;
        yield return new WaitForSeconds(_shootingDelays.x);

        _shootingManager.Shoot(transform, _shootingSpreadRange, _bulletPower);
        yield return new WaitForSeconds(_shootingDelays.y);

        _navMeshAgent.Warp(transform.position);
        _shootingTimer = 0;
        _navMeshAgent.updatePosition = true;

    }

}
