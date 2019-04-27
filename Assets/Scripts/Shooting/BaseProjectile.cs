using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour {

    [Header("Projectile variables")]
    [Tooltip("Speed of the projectile")]
    [SerializeField] float _speed;
    public float Speed => _speed;

    void OnTriggerEnter (Collider other) {

        if (!other.CompareTag("Enemy")) {

            if (other.CompareTag("Player"))
                print("Player hit");

            gameObject.SetActive(false);

        }

    }

}
