using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRangedEnemy : BaseEnemy {


    [Header("Shooting variables")]
    protected ShootingManager _shootingManager;

    [Tooltip("Damage of this enemy's attack")]
    [SerializeField] protected int _damage;

    [Tooltip("Range for the \"spread\" of the shooting")]
    [SerializeField] protected Vector2 _shootingSpreadRange;

    [Tooltip("Time before shooting after having detected the player")]
    [SerializeField] protected float _shootingDelay;

    [Tooltip("Fire rate (time between two shots)")]
    [SerializeField] protected float _fireRate;

    public override void Initialization () {

        base.Initialization();
        _shootingManager = FindObjectOfType<ShootingManager>();
    }

    protected override void PlayerDetected () {        

        if (!_playerDetected) {
            base.PlayerDetected();
            _navMeshAgent.updatePosition = false;
            StartCoroutine("Shoot");            
        }

        if (_navMeshAgent.enabled)
        {
            _navMeshAgent.SetDestination(transform.position + (_player.transform.position - transform.position));
        }
        else
        {
            _character.FireStopWalk();
        }

    }

    protected IEnumerator Shoot () {

        yield return new WaitForSeconds(_shootingDelay);

        while (true) {
            _character.EventFire();
            if(_player!=null) _character.transform.LookAt(_player.transform.position);
            _shootingManager.Shoot(transform, "Player", _shootingSpreadRange, _damage, 0);            
            yield return new WaitForSeconds(_fireRate);
        }

    }

    protected override void PlayerLost () {

        base.PlayerLost();
        StopCoroutine("Shoot");
        _character.EventFireStop();
        _navMeshAgent.Warp(transform.position);
        _navMeshAgent.updatePosition = true;

    }

}
