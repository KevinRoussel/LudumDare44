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

    private void Start()
    {
        _shootDown = new Trigger();
        _shootUp = new Trigger();
        _inputRoutine = StartCoroutine(InputRoutine());

        IEnumerator InputRoutine()
        {
            while (true)
            {
                _move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                if (Input.GetMouseButtonUp(0)) _shootUp.Activate();
                if (Input.GetMouseButtonDown(0)) _shootDown.Activate();
                yield return null;
                ResetInput();
            }
        }
    }
    public void ResetInput()
    {
        _shootUp.IsActivated();
        _shootDown.IsActivated();
        _move = Vector2.zero;
    }

    public void ApplyInput(Character control)
    {
        control.Move(_move);
        if (_shootUp.IsActivated()) control.StopAttack();
        if (_shootDown.IsActivated()) control.LaunchAttack();
    }
}
