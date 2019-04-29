using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadEnding : EndingScenario
{
    protected override IEnumerator RunScenario() {
        yield return base.RunScenario();

        _dialog.ChangeName("SATAN");
        _dialog.ChangeDialogText("AND WE HAVE THE GREAT PRIVILEGE TO ANNOUNCE THAT YOU FAILED IN YOUR QUEST FOR FREEDOM.");
        yield return UserAction();

        _dialog.ChangeName("LEVIATHAN");
        _dialog.ChangeDialogText("JUST LIKE FOR MANY OTHERS BEFORE YOU, WE ACHIEVED TO CONSUME YOUR LIFE AND CORRUPT YOUR SOUL DURING YOUR QUEST.");
        yield return UserAction();

        _dialog.ChangeName("BILAL");
        _dialog.ChangeDialogText("YOU DOOMED YOURSELF BY YOUR ACTIONS. YOU SOLD YOUR LIFE TO US.");
        yield return UserAction();

        _dialog.ChangeDialogText("NOW YOU ARE OUR MINION FOR ETERNITY.");
        yield return UserAction();

    }
}
