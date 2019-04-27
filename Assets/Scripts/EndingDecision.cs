using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingDecision : MonoBehaviour
{
    [SerializeField] int evilPoints;

    [SerializeField] int badEndingThreshold;

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
        Debug.Log("Bad Ending");
    }

    void CallGoodEnding() {
        Debug.Log("Good Ending");
    }
}
