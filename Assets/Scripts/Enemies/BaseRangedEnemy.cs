using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRangedEnemy : BaseEnemy {

    [Header("Shooting variables")]
    protected ShootingManager _shootingManager;

    [Tooltip("Range for the \"spread\" of the shooting")]
    [SerializeField] protected Vector2 _shootingSpreadRange;

    [Tooltip("Time before shooting after having detected the player")]
    [SerializeField] protected float _shootingDelay;

    [Tooltip("Fire rate (time between two shots)")]
    [SerializeField] protected float _fireRate;

    public override void Initialization () {

        base.Initialization();

        _shootingManager = FindObjectOfType<ShootingManager>();

        _character.StartDeath += () => {

            StopCoroutine("RotateToPlayer");

            StopCoroutine("Shoot");

        };

    }

    protected override void PlayerDetected () {

        if (_navMeshAgent.enabled && !_playerDetected) {

            base.PlayerDetected();

            SetCanMove(false);

            _animator.SetBool("Firing", true);

            StartCoroutine("Shoot");

            StartCoroutine("RotateToPlayer");

        }

    }

    protected IEnumerator RotateToPlayer () {

        while (true) {

            if (_player)
                _character.transform.LookAt(_player.transform.position);

            yield return new WaitForEndOfFrame();

        }

    }

    protected IEnumerator Shoot () {

        yield return new WaitForSeconds(_shootingDelay);

        while (true) {

            _shootingManager.Shoot(transform, "Player", _shootingSpreadRange, _character.Attack, 0);     
            
            yield return new WaitForSeconds(_fireRate);

        }

    }

    protected override void PlayerLost () {

        base.PlayerLost();

        StopCoroutine("RotateToPlayer");

        StopCoroutine("Shoot");

        _character.EventFireStop();

        if (MovingEnemy)
            SetCanMove(true);

    }

}
