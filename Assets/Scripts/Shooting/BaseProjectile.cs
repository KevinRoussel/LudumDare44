using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseProjectile : MonoBehaviour {

    [Header("Projectile variables")]
    [Tooltip("Speed of the projectile")]
    [SerializeField] float _speed;
    [SerializeField] float _initialSpeed;

    [Tooltip("Power of the projectile (will mostly been overriden by instigator)")]
    [SerializeField] int _power;

    [Tooltip("Maximum lifetime of the projectile")]
    [SerializeField] float _lifetime;

    [SerializeField] MeshRenderer _projectileRenderer;
    [SerializeField] Light _light;
    [SerializeField] UnityEvent _onContactEvent;

    public float Speed { get => _speed; set => _speed = value; }
    public int Power { get => _power; set => _power = value; }

    private void Awake()
    {
        _initialSpeed = _speed;
    }

    public string TargetTag { get; set; }

    public Character Instigator { get; set; }



    void OnEnable () {
        _projectileRenderer.enabled = true;
        StartCoroutine("LifetimeDeactivation");
        _speed = _initialSpeed;
    }

    void OnDisable () {
        StopCoroutine("LifetimeDeactivation");
    }

    void OnTriggerEnter (Collider other) {

        if (TargetTag!="" && other.CompareTag(TargetTag))
        {
            _onContactEvent?.Invoke();
            _projectileRenderer.enabled = false;
            StartCoroutine(ContactEffect());
            other.GetComponent<Character>()?.Hit(Instigator, _power);
        }
    }

    IEnumerator ContactEffect()
    {
        _speed = 0f;
        _light.enabled = true;
        yield return new WaitForSeconds(0.1f);
        _light.enabled = false;

        yield break;
    }

    IEnumerator LifetimeDeactivation () {

        yield return new WaitForSeconds(_lifetime);

        gameObject.SetActive(false);

    }

}
