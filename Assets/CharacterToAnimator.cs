using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterToAnimator : MonoBehaviour
{
    [Header("Dependance")]
    [SerializeField] Character _character;
    [SerializeField] Animator _animator;

    [Header("Animator Parameters")]
    [SerializeField] string _firingBool = "Firing";
    [SerializeField] string _walkingBool = "Walking";
    [SerializeField] string _shieldBool = "Shield";
    [SerializeField] string _damageTrigger = "Damage";
    [SerializeField] string _deadTrigger = "Dead";

    private void Start()
    {
        _character.OnAttack += () => _animator.SetBool(_firingBool, true);
        _character.OnStopAttack += () => _animator.SetBool(_firingBool, false);

        _character.StartWalking += () => _animator.SetBool(_walkingBool, true);
        _character.EndWalking += () => _animator.SetBool(_walkingBool, false);

        _character.OnShieldOn += () => _animator.SetBool(_shieldBool, true);
        _character.OnShieldOff += () => _animator.SetBool(_shieldBool, false);

        _character.OnTakeDamage += (i) => _animator.SetTrigger(_damageTrigger);
        _character.StartDeath += () => _animator.SetTrigger(_deadTrigger);

    }


}
