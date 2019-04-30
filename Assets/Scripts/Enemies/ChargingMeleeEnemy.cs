using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingMeleeEnemy : BaseEnemy {

    [Header("Charging enemy variables")]
    [Tooltip("Speed is multiplied by this value while the enemy charges the player")]
    [SerializeField] float _chargeSpeedMultiplier;

    [Tooltip("Attack range")]
    [SerializeField] float _attackRange;

    [Tooltip("Attack time")]
    [SerializeField] float _attackTime;
    float _attackTimer;

    public override void Tick () {

        _attackTimer -= (_attackTimer != -1) ? Time.deltaTime : 0;

        base.Tick();

    }

    protected override void PlayerDetected () {

        if (!_playerDetected && _navMeshAgent.enabled) {

            base.PlayerDetected();

            _navMeshAgent.speed *= _chargeSpeedMultiplier;

            _navMeshAgent.SetDestination(_player.transform.position);

            _animator.SetBool("Walking", true);

        }

        if ((_navMeshAgent.remainingDistance <= _attackRange) && (_attackTimer <= 0)) {

            _attackTimer = -1;
            _character.EventFire();
            StartCoroutine("Attack");

        }

    }

    protected override void PlayerLost () {

        _navMeshAgent.speed /= _chargeSpeedMultiplier;
        base.PlayerLost();

    }

    IEnumerator Attack () {

        SetCanMove(false);

        // _character.EventFire();

        yield return new WaitForSeconds(1);

        // _character.EventFireStop();

        SetCanMove(true);

        _attackTimer = _attackTime;

    }

}
