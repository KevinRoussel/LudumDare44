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
        _dialog.ChangeDialogText("SO WHAT SHOULD WE DO OF THIS BORING LIGHTHEARTED SOUL?");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("SATAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[0]._voice);
        _dialog.ChangeDialogText("IDK, THEY'RE BORIN AS HELL");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("LEVIATHAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[1]._voice);
        _dialog.ChangeDialogText("WE'RE IN HELL SATAN");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("SATAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[0]._voice);
        _dialog.ChangeDialogText("I KNOW RIGHT THAT'S WHAT I AM SAYING");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("BILAL");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[2]._voice);
        _dialog.ChangeDialogText("THEY MIGHT BRING EVEN MORE BOREDOM IN THE UNDERWORLD");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("SATAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[0]._voice);
        _dialog.ChangeDialogText("HOW WOULD THAT BE POSSIBLE? WITH AN EVEN LESS INTERESTING GAME?");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("LEVIATHAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[1]._voice);
        _dialog.ChangeDialogText("IMAGINE IF THEY BRING PBR GRAPHICS HERE...");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("SATAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[0]._voice);
        _dialog.ChangeDialogText("OH NO! NOT PBR GRAPHICS! I DON'T WANT ALL OF THIS TO BECAME YET ANOTHER BORING SEQUEL OF ASSASSIN'S CREED!");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("BILAL");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[2]._voice);
        _dialog.ChangeDialogText("WE SHOULD LET HIM MAKE THE HUMAN WORLD EVEN MORE BORING I GUESS?");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("LEVIATHAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[1]._voice);
        _dialog.ChangeDialogText("SO YOU MEAN LIKE, TRIGGERING THE GOOD ENDING? THEY REALLY WERE THIS BORING?");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("BILAL");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[2]._voice);
        _dialog.ChangeDialogText("YEAH AND THANKFULLY THERE ARE ONLY THREE LEVELS, SINCE IT'S ONLY THE JAM VERSION...");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("SATAN");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[0]._voice);
        _dialog.ChangeDialogText("LET'S JUST DO THIS, I CAN'T STAND THIS GAME ANYMORE I WANT TO GO TO SLEEP");
        yield return WaitForTypingFinished();
        _dialog.ChangeName("BILAL");
        _dialog.TypeWriter.ChangeSound(_gameplay.MapStructure[0].Pacts[2]._voice);
        _dialog.ChangeDialogText("OK");
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
        yield return _cinematic.PlayAndWait(_goodEndingClip);
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
