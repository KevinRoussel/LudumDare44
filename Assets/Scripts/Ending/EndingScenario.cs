using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingScenario : MonoBehaviour
{
    [SerializeField] protected GameObject _characterSlots;
    [SerializeField] protected Dialog _dialog;
    [SerializeField] protected Light _light;
    
    public void Activate() {
        StartCoroutine(RunScenario());
    }

    protected virtual IEnumerator RunScenario() {
        yield return new WaitForSeconds(3f);
        _dialog.ChangeName("????");
        _dialog.ChangeDialogText("Hehehe... So here you are...".ToUpper());
        _dialog.Toggle();
        yield return UserAction();

        _dialog.ChangeDialogText("Come in, person.".ToUpper());
        yield return UserAction();

        _dialog.Toggle();
        yield return new WaitForSeconds(1.5f);
        _light.enabled = true;
        yield return new WaitForSeconds(1.5f);
        _characterSlots.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        _dialog.ChangeName("SATAN");
        _dialog.ChangeDialogText("CONGRATULATIONS, PERSON. YOU MADE YOUR WAY UP HERE...");
        _dialog.Toggle();
        yield return UserAction();

        _dialog.ChangeName("LEVIATHAN");
        _dialog.ChangeDialogText("DO YOU KNOW WHAT WE ARE?");
        yield return UserAction();

        _dialog.ChangeName("BILAL");
        _dialog.ChangeDialogText("THE THREE PRIMARY DEMONS.");
        yield return UserAction();
        _dialog.ChangeDialogText("LORDS OF THIS UNDERWORLD.");
        yield return UserAction();

        _dialog.ChangeName("LEVIATHAN");
        _dialog.ChangeDialogText("WE ARE THE LAST GATE BETWEEN THIS WORLD AND YOURS.");
        yield return UserAction();
    }

    public IEnumerator UserAction() {
        yield return new WaitUntil(() => !_dialog.IsTyping() && (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)));
    }
}
