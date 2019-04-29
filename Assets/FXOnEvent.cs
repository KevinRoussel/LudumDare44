using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXOnEvent : MonoBehaviour
{
    [Header("Master")]
    [SerializeField] Character _target;

    [Header("SFX")]
    [SerializeField] ParticleSystem _rageActivated;
    [SerializeField] ParticleSystem _shieldHit;
    [SerializeField] Animation _flashAnimation;
    [SerializeField] ParticleSystem _flashed;

    private void Start()
    {
        _target.OnRageReady += () => _rageActivated?.Play();
        _target.OnShieldHit += () => _shieldHit?.Play();
        _target.OnFlashLauched += () => _flashAnimation?.Play();
        _target.OnFlashed += () => _flashed?.Play();

    }




}
