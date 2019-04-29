﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingDecision : MonoBehaviour
{
    [SerializeField] int badEndingThreshold;

    [SerializeField] BadEnding badEnding;
    [SerializeField] GoodEnding goodEnding;

    [SerializeField] List<Transform> _toDesactivate;

    int _evilPoints=0;

    public int CurrentEvilPoint => _evilPoints;
    public void AddEvilPoint() => _evilPoints++;

    public IEnumerator StartEnding()
    {
        if (_evilPoints > badEndingThreshold) {
            yield return CallBadEnding();
        } else {
            yield return CallGoodEnding();
        }
    }

    IEnumerator CallBadEnding() {
        _toDesactivate.ForEach(i => i.gameObject.SetActive(false));
        badEnding.gameObject.SetActive(true);
        yield return badEnding.Activate();
        _toDesactivate.ForEach(i => i.gameObject.SetActive(true));
    }

    IEnumerator CallGoodEnding() {
        _toDesactivate.ForEach(i => i.gameObject.SetActive(false));
        goodEnding.gameObject.SetActive(true);
        yield return goodEnding.Activate();
        _toDesactivate.ForEach(i => i.gameObject.SetActive(true));
    }
}
