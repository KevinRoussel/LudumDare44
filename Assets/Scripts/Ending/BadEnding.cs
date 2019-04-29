using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadEnding : EndingScenario
{
    [Header("Animations")]
    [SerializeField] Animation _cinematic;
    [SerializeField] AnimationClip _badEndingClip;

    protected override IEnumerator RunScenario() {
        yield return base.RunScenario();

        _dialog.ChangeName("SATAN");
        ChangeScale(SATAN: true, LEVIATHAN: false, BILAL: false);
        _dialog.ChangeDialogText("AND WE HAVE THE GREAT PRIVILEGE TO ANNOUNCE THAT YOU FAILED IN YOUR QUEST FOR FREEDOM.");
        yield return WaitForUserAction();

        _dialog.ChangeName("LEVIATHAN");
        ChangeScale(SATAN: false, LEVIATHAN: true, BILAL: false);
        _dialog.ChangeDialogText("JUST LIKE FOR MANY OTHERS BEFORE YOU, WE ACHIEVED TO CONSUME YOUR LIFE AND CORRUPT YOUR SOUL DURING YOUR QUEST.");
        yield return WaitForUserAction();

        _dialog.ChangeName("BILAL");
        ChangeScale(SATAN: false, LEVIATHAN: false, BILAL: true);
        _dialog.ChangeDialogText("YOU DOOMED YOURSELF BY YOUR ACTIONS. YOU SOLD YOUR LIFE TO US.");
        yield return WaitForUserAction();

        _dialog.ChangeDialogText("NOW YOU ARE OUR SLAVE FOR ETERNITY.");
        yield return WaitForUserAction();
        _dialog.Toggle();

        yield return _cinematic.PlayAndWait(_badEndingClip);
        yield return new WaitForSeconds(2f);

        _dialog.ChangeDialogText("YOU'RE LIVING IN A WORLD WHERE YOUR ACTIONS HAVE CONSEQUENCES, SLAVE.");
        _dialog.Toggle();
        yield return WaitForUserAction();
        _dialog.ChangeDialogText("MANY BEFORE YOU HAVE TRIED TO ESCAPE. THEY ALL FAILED, JUST LIKE YOU.");
        yield return WaitForUserAction();
        _dialog.ChangeDialogText("PERHAPS SOMEONE ELSE WILL GRASP THIS FACT AND WON'T SPEND ALL THEIR LIFE ESSENCE SEEKING DEATH.");
        yield return WaitForUserAction();
        _dialog.Toggle();
    }
}
