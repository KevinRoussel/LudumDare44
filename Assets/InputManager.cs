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
    Vector2 _mousePosition;
    bool _skillDown = false;
    //Trigger _mouse

    Vector2 _offset;
    public void SetOffset(Vector2 v) => _offset = v;

    [Range(0f, 1f)]
    [SerializeField] float offsetForce = 0.5f;

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
                _mousePosition = Input.mousePosition;
                if (Input.GetMouseButtonUp(0)) _shootUp.Activate();
                if (Input.GetMouseButtonDown(0)) _shootDown.Activate();

                if (Input.GetMouseButtonDown(1)) _skillDown = true;
                if (Input.GetMouseButtonUp(1)) _skillDown = false;
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
        control.Move(_move + new Vector2(control.ComputeOffset().x, control.ComputeOffset().z) * offsetForce);
        if (_shootUp.IsActivated()) control.StopAttack();
        if (_shootDown.IsActivated()) control.LaunchAttack();
        if (_skillDown) control.LaunchSkill();
        control.LookAt(_mousePosition);
    }
}
