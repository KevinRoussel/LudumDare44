using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCharacter : MonoBehaviour
{
    [Header("Master")]
    [SerializeField] Character _target;

    [Header("SFX")]
    AudioSource _rageActivated;

    private void Start()
    {
        //_target.OnRageReady += () => _rageActivated.Play();



    }




}
