using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingMeleeEnemy : BaseEnemy {

    float _baseSpeed;

    [Header("Charging enemy variables")]
    [Tooltip("Speed at which the enemy charges the player")]
    [SerializeField] float _chargeSpeed;

    [Tooltip("Attack range")]
    [SerializeField] float _attackRange;

    [Tooltip("Attack time")]
    [SerializeField] float _attackTime;
    float _attackTimer;

    public override void Initialization () {

        base.Initialization();

        _baseSpeed = _navMeshAgent.speed;

    }

    public override void Tick () {

        _attackTimer -= (_attackTimer != -1) ? Time.deltaTime : 0;

        base.Tick();

    }

    protected override void PlayerDetected () {

        if (!_playerDetected) {

            base.PlayerDetected();

            _navMeshAgent.speed = _chargeSpeed;

            if (_navMeshAgent.enabled)
                _navMeshAgent.SetDestination(_player.transform.position);

        }

        if ((_navMeshAgent.remainingDistance <= _attackRange) && (_attackTimer <= 0)) {

            _attackTimer = -1;

            StartCoroutine("Attack");

        }

    }

    protected override void PlayerLost () {

        _navMeshAgent.speed = _baseSpeed;

        base.PlayerLost();

    }

    IEnumerator Attack () {

        SetCanMove(false);

        yield return new WaitForSeconds(1);

        SetCanMove(true);

        _attackTimer = _attackTime;

    }

}
