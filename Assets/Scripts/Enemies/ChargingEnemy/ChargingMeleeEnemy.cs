using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingMeleeEnemy : BaseEnemy {

    [Header("Charging enemy variables")]
    [Tooltip("Speed is multiplied by this value while the enemy charges the player")]
    [SerializeField] float _chargeSpeedMultiplier;

    ChargingMeleeEnemyAttackZone _head;

    [Tooltip("Delay between movement stopping and attack starting and between two attacks")]
    [SerializeField] float _attackDelay;
    float _attackTimer;

    [Tooltip("Attack duration")]
    [SerializeField] float _attackDuration;    

    public override void Initialization () {

        base.Initialization();

        _head = GetComponentInChildren<ChargingMeleeEnemyAttackZone>();

        _attackTimer = _attackDelay;

    }

    public override void Tick () {

        _attackTimer += (_attackTimer != -1) ? Time.deltaTime : 0;

        base.Tick();

    }

    protected override void PlayerDetected () {

        if (!_playerDetected && _navMeshAgent.enabled) {

            base.PlayerDetected();

            _navMeshAgent.speed *= _chargeSpeedMultiplier;            

            _animator.SetBool("Walking", true);

        }

        _navMeshAgent.SetDestination(_player.transform.position);

        if ((Vector3.Distance(transform.position, _player.transform.position) <= _navMeshAgent.stoppingDistance) && (_attackTimer >= _attackDelay)) {

            _attackTimer = -1;

            SetCanMove(false);

            StartCoroutine("Attack");

        }

    }

    protected override void PlayerLost () {

        SetDestination();

        _navMeshAgent.speed /= _chargeSpeedMultiplier;

        base.PlayerLost();

    }

    IEnumerator Attack () {

        yield return new WaitForSeconds(_attackDelay);

        _head.Collider.enabled = true;

        _animator.SetBool("Firing", true);

        yield return new WaitForSeconds(_attackDuration);

        _attackTimer = 0;

        _head.Collider.enabled = false;

        _animator.SetBool("Firing", false);

        SetCanMove(true);

    }

}
