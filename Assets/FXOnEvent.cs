using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXOnEvent : MonoBehaviour
{
    [Header("Master")]
    [SerializeField] Character _target;

    [Header("SFX")]
    ParticleSystem _rageActivated;

    private void Start()
    {
        _target.OnRageReady += () => _rageActivated.Play();



    }

}
