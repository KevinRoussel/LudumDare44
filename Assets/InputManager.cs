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
                if (Input.GetMouseButtonDown(0)) _shootDown.Activate();
                if (Input.GetMouseButtonUp(0)) _shootUp.Activate();
                yield return null;
            }
        }
    }

    public void ApplyInput(Character control)
    {
        if (_move.magnitude > 0.001f)
            control.Move(_move);
        if (_shootUp.IsActivated()) control.StopAttack();
        if (_shootDown.IsActivated()) control.LaunchAttack(Input.mousePosition);
    }
}
