using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingRangedEnemy : BaseRangedEnemy {

    [Tooltip("Chasing radiu")]
    [SerializeField] protected float _chasingRadius;

    void OnValidate () {

        _chasingRadius = Mathf.Max(_chasingRadius, _playerDetectionDistance + 1);

    }

    protected override void PlayerLost () {

        base.PlayerLost();

        SetChasingDestination();

    }

    bool SetChasingDestination () {

        if (Vector3.Distance(_player.transform.position, transform.position) <= _chasingRadius) {

            SetNavDestination(_player.transform.position + ((transform.position - _player.transform.position).normalized * _playerDetectionDistance * .8f));

            return true;

        } else
            return false;

    }

    protected override void Movement () {

        if(!SetChasingDestination()) {

            SetDestination();

            base.Movement();

        }            

    }

}
