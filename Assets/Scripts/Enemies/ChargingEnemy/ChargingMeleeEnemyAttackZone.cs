using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingMeleeEnemyAttackZone : MonoBehaviour {

    Character _character;

    Collider _collider;
    public Collider Collider { get => _collider; set => _collider = value; }

    void Awake () {

        _character = GetComponentInParent<Character>();

        Collider = GetComponent<Collider>();

    }

    void OnTriggerEnter (Collider other) {
        
        if(other.CompareTag("Player")) {

            Collider.enabled = false;

            other.GetComponent<Character>().Hit(_character, _character.Attack);

        }

    }

}
