using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingDecision : MonoBehaviour
{
    [SerializeField] int evilPoints;

    [SerializeField] int badEndingThreshold;

    private EndingScenario badEnding;
    private EndingScenario goodEnding;

    void Awake() {
        badEnding = GetComponent<BadEnding>();
        goodEnding = GetComponent<GoodEnding>();
    }

    // TODO : Delete Start function and integrate ending decision in the coroutine process
    void Start()
    {
        if (evilPoints > badEndingThreshold) {
            CallBadEnding();
        } else {
            CallGoodEnding();
        }

    }

    void CallBadEnding() {
        badEnding.Activate();
    }

    void CallGoodEnding() {
        goodEnding.Activate();
    }
}
