using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour {

    [Header("Projectile variables")]
    [Tooltip("Speed of the projectile")]
    [SerializeField] float _speed;
    [SerializeField] int _power;

    public float Speed { get => _speed; set => _speed = value; }
    public int Power { get => _power; set => _power = value; }

    public string TargetTag { get; set; }

    void OnTriggerEnter (Collider other) {

        if (other.CompareTag(TargetTag))
        {
            gameObject.SetActive(false);
            other.GetComponent<Character>()?.Hit(_power);
        }

    }

}
