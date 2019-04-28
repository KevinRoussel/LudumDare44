using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour {

    [Header("Projectile variables")]
    [Tooltip("Speed of the projectile")]
    [SerializeField] float _speed;

    [Tooltip("Power of the projectile (will mostly been overriden by instigator)")]
    [SerializeField] int _power;

    [Tooltip("Maximum lifetime of the projectile")]
    [SerializeField] float _lifetime;

    public float Speed { get => _speed; set => _speed = value; }
    public int Power { get => _power; set => _power = value; }

    public string TargetTag { get; set; }

    void OnEnable () {

        StartCoroutine("LifetimeDeactivation");

    }

    void OnDisable () {

        StopCoroutine("LifetimeDeactivation");

    }

    void OnTriggerEnter (Collider other) {

        if (other.CompareTag(TargetTag))
        {
            gameObject.SetActive(false);
            other.GetComponent<Character>()?.Hit(_power);
        }

    }

    IEnumerator LifetimeDeactivation () {

        yield return new WaitForSeconds(_lifetime);

        gameObject.SetActive(false);

    }

}
