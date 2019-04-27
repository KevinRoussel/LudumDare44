using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour {

    [Header("Projectile variables")]
    [Tooltip("Speed of the projectile")]
    [SerializeField] float _speed;
    [SerializeField] int _power;


    public float Speed => _speed;
    public int Power { get => _power; set => _power = value; }

    void OnTriggerEnter (Collider other) {

        if (!other.CompareTag("Enemy")) {

            if (other.CompareTag("Player"))
            {
                gameObject.SetActive(false);
                other.GetComponent<Character>().Hit(_power);
            }

        }

    }

}
