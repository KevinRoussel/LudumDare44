using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodEnding : EndingScenario
{
    [Header("Animations")]
    [SerializeField] Animation _cinematic;
    [SerializeField] AnimationClip _goodEndingClip;

    protected override IEnumerator RunScenario() {
        yield return base.RunScenario();


    }
}
