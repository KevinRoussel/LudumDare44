using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingScenario : MonoBehaviour
{

    [Header("Managers")]
    [SerializeField] protected Gameplay _gameplay;

    [SerializeField] protected GameObject _characterSlots;
    [SerializeField] protected Dialog _dialog;
    [SerializeField] protected Light _light;
    [SerializeField] protected CanvasGroup _background;

    [Header("Slots")]
    [SerializeField] RectTransform _SATAN;
    [SerializeField] RectTransform _LEVIATHAN;
    [SerializeField] RectTransform _BILAL;
    [SerializeField] float _minScale;
    [SerializeField] float _targetScale;


    protected void ChangeScale(bool SATAN, bool LEVIATHAN, bool BILAL)
    {
        _SATAN.localScale = new Vector3(SATAN? _targetScale:_minScale, SATAN ? _targetScale : _minScale, SATAN ? _targetScale : _minScale);
        _LEVIATHAN.localScale = new Vector3(LEVIATHAN ? _targetScale:_minScale, LEVIATHAN ? _targetScale : _minScale, LEVIATHAN ? _targetScale : _minScale);
        _BILAL.localScale = new Vector3(BILAL ? _targetScale:_minScale, BILAL ? _targetScale : _minScale, BILAL ? _targetScale : _minScale);
    }
    
    public IEnumerator Activate() {
        yield return StartCoroutine(RunScenario());
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
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[0]._voice);
        ChangeScale(SATAN: true, LEVIATHAN: false, BILAL: false);
        _dialog.ChangeDialogText("CONGRATULATIONS, PERSON. YOU MADE YOUR WAY UP HERE...");
        _dialog.Toggle();
        yield return UserAction();

        _dialog.ChangeName("LEVIATHAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[1]._voice);
        ChangeScale(SATAN: false, LEVIATHAN: true, BILAL: false);
        _dialog.ChangeDialogText("DO YOU KNOW WHAT WE ARE?");
        yield return UserAction();

        _dialog.ChangeName("BILAL");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[2]._voice);
        ChangeScale(SATAN: false, LEVIATHAN: false, BILAL: true);
        _dialog.ChangeDialogText("THE THREE PRIMARY DEMONS.");
        yield return UserAction();
        _dialog.ChangeDialogText("LORDS OF THIS UNDERWORLD.");
        yield return UserAction();

        _dialog.ChangeName("LEVIATHAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[1]._voice);
        ChangeScale(SATAN: false, LEVIATHAN: true, BILAL: false);
        _dialog.ChangeDialogText("WE ARE THE LAST GATE BETWEEN THIS WORLD AND YOURS.");
        yield return UserAction();
    }

    public IEnumerator UserAction() {
        yield return new WaitUntil(() => !_dialog.IsTyping() && (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)));
    }
}
