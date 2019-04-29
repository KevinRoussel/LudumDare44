using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingDecision : MonoBehaviour
{
    [SerializeField] int evilPoints;

    [SerializeField] int badEndingThreshold;

    private EndingScenario ending;

    // TODO : Delete Start function and integrate ending decision in the coroutine process

    void Awake() {
        ending = GetComponent<EndingScenario>();
    }

    void Start()
    {
        if (evilPoints > badEndingThreshold) {
            CallBadEnding();
        } else {
            CallGoodEnding();
        }

        Debug.Log(ending);
    }

    void CallBadEnding() {
        Debug.Log("Bad Ending");
        ending.Activate();
    }

    void CallGoodEnding() {
        Debug.Log("Good Ending");
        ending.Activate();
    }
}
