using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodEnding : EndingScenario
{
    [Header("Animations")]
    [SerializeField] Animation _cinematic;
    [SerializeField] AnimationClip _goodEndingClip;

    [SerializeField] Light exitLight;

    protected override IEnumerator RunScenario() {
        yield return base.RunScenario();

        _dialog.ChangeName("SATAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[0]._voice);
        _dialog.ChangeDialogText("AND WE HAVE THE GREAT PRIVILEGE TO ANNOUNCE THAT YOU...");
        ChangeScale(SATAN: true, LEVIATHAN: false, BILAL: false);
        yield return WaitForUserAction();
        _dialog.ChangeName("LEVIATHAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[1]._voice);
        _dialog.ChangeDialogText("... HAVE BEEN VERY SURPRISING.");
        ChangeScale(SATAN: false, LEVIATHAN: true, BILAL: false);
        yield return WaitForUserAction();

        _dialog.ChangeName("SATAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[0]._voice);
        _dialog.ChangeDialogText("DESPITE ALL OUR EFFORTS TO CORRUPT YOU, YOUR SOUL IS STILL PURE AND UNTOUCHED.");
        ChangeScale(SATAN: true, LEVIATHAN: false, BILAL: false);
        yield return WaitForUserAction();
        _dialog.ChangeName("LEVIATHAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[1]._voice);
        _dialog.ChangeDialogText("WHAT SHOULD WE DO WITH YOU NOW??");
        ChangeScale(SATAN: false, LEVIATHAN: true, BILAL: false);
        yield return WaitForUserAction();
        _dialog.ChangeName("BILAL");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[2]._voice);
        _dialog.ChangeDialogText("HMM...");
        ChangeScale(SATAN: false, LEVIATHAN: false, BILAL: true);
        yield return WaitForUserAction();

        _dialog.Toggle();
        ChangeScale(SATAN: false, LEVIATHAN: false, BILAL: false);
        yield return new WaitForSeconds(2f);

        _dialog.Toggle();
        _dialog.ChangeName("BILAL");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[2]._voice);
        _dialog.ChangeDialogText("so what should we do of this boring lighthearted soul?");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("SATAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[0]._voice);
        _dialog.ChangeDialogText("idk, they're borin as hell");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("LEVIATHAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[1]._voice);
        _dialog.ChangeDialogText("we're in hell satan");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("SATAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[0]._voice);
        _dialog.ChangeDialogText("i know right that's what i am saying");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("BILAL");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[2]._voice);
        _dialog.ChangeDialogText("they might bring even more boredom in the underworld");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("SATAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[0]._voice);
        _dialog.ChangeDialogText("how would that be possible? with an even less interesting game?");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("LEVIATHAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[1]._voice);
        _dialog.ChangeDialogText("imagine if they bring pbr graphics here...");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("SATAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[0]._voice);
        _dialog.ChangeDialogText("oh no! not pbr graphics! i don't want all of this to became yet another boring sequel of assassin's creed!");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("BILAL");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[2]._voice);
        _dialog.ChangeDialogText("we should let him make the human world even more boring i guess?");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("LEVIATHAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[1]._voice);
        _dialog.ChangeDialogText("so you mean like, triggering the good ending? they really were this boring?");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("BILAL");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[2]._voice);
        _dialog.ChangeDialogText("yeah and thankfully there are only three levels, since it's only the jam version...");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("SATAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[0]._voice);
        _dialog.ChangeDialogText("let's just do this, i can't stand this game anymore i want to go to sleep");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("BILAL");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[2]._voice);
        _dialog.ChangeDialogText("kk");
        yield return WaitForTypingFinished();
        yield return new WaitForSeconds(1f);
        _dialog.Toggle();


        yield return new WaitForSeconds(3f);
        _dialog.ChangeDialogText("");
        _dialog.Toggle();
        ChangeScale(SATAN: false, LEVIATHAN: false, BILAL: true);
        _dialog.ChangeName("BILAL");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[2]._voice);
        _dialog.ChangeDialogText("PERSON...");
        yield return WaitForUserAction();
        _dialog.ChangeDialogText("WE HAVE STATED ON YOUR FATE.");
        yield return WaitForUserAction();

        _dialog.ChangeName("LEVIATHAN");
        ChangeScale(SATAN: false, LEVIATHAN: true, BILAL: false);
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[1]._voice);
        _dialog.ChangeDialogText("YOUR ACTIONS SHOWED THAT YOUR SOUL DOES NOT BELONG HERE.");
        yield return WaitForUserAction();

        _dialog.ChangeName("SATAN");
        ChangeScale(SATAN: true, LEVIATHAN: false, BILAL: false);
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[0]._voice);
        _dialog.ChangeDialogText("WE DECIDED TO LET YOU GO WHERE YOU'RE FROM.");
        yield return WaitForUserAction();

        _dialog.ChangeName("LEVIATHAN");
        ChangeScale(SATAN: false, LEVIATHAN: true, BILAL: false);
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[1]._voice);
        _dialog.ChangeDialogText("JUST GO THROUGH THE GATE, HERE.");
        yield return WaitForUserAction();
        _dialog.Toggle();

        yield return new WaitForSeconds(1f);
        exitLight.enabled = true;
        yield return new WaitForSeconds(1f);

        _dialog.ChangeDialogText("");
        _dialog.Toggle();
        _dialog.ChangeName("SATAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[0]._voice);
        ChangeScale(SATAN: true, LEVIATHAN: false, BILAL: false);
        _dialog.ChangeDialogText("AND ALSO, IF YOU COULD HURRY UP. WE HAVE MANY OTHER SOULS TO DOOM TODAY. THANK YOU.");
        yield return WaitForUserAction();

    }
}
