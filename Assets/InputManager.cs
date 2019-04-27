using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] Master _master;

    Coroutine _inputRoutine;

    Trigger _shootUp;
    Trigger _shootDown;
    Vector2 _move;

    private void Awake()
    {
        _shootDown = new Trigger();
        _shootUp = new Trigger();
        _inputRoutine = StartCoroutine(InputRoutine());

        IEnumerator InputRoutine()
        {
            _move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (Input.GetMouseButtonDown(1)) _shootDown.Activate();
            if (Input.GetMouseButtonDown(1)) _shootUp.Activate();
            yield break;
        }
    }

    public void ApplyInput(Character control)
    {
        if (_move.magnitude > 0.001f) control.Move(_move);
        if (_shootUp.IsActivated()) control.LaunchAttack();
        if (_shootDown.IsActivated()) control.StopAttack();
    }

    






}
