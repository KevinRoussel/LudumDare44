using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingScenario : MonoBehaviour
{
    [SerializeField] GameObject _characterSlots;
    [SerializeField] Dialog dialog;
    [SerializeField] Light light;
    public void Activate() {
        StartCoroutine(RunScenario());
    }

    protected virtual IEnumerator RunScenario() {
        yield return new WaitForSeconds(3f);
        dialog.ChangeName("????");
        dialog.ChangeDialogText("Hehehe... So here you are...".ToUpper());
        dialog.Toggle();
        yield return new WaitUntil(() => UserAction());

        dialog.ChangeDialogText("Come in, person.".ToUpper());

    }

    public bool UserAction() {
        return !dialog.IsTyping() && (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0));
    }
}
