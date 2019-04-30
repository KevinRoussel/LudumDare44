using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingDecision : MonoBehaviour
{

    [SerializeField] int badEndingThreshold;

    [SerializeField] BadEnding badEnding;
    [SerializeField] GoodEnding goodEnding;

    [SerializeField] List<Transform> _toDesactivate;

    [SerializeField] int _evilPoints=0;

    public int CurrentEvilPoint => _evilPoints;
    public void AddEvilPoint(int amount) => _evilPoints+=amount;

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
    }

    IEnumerator CallGoodEnding() {
        _toDesactivate.ForEach(i => i.gameObject.SetActive(false));
        goodEnding.gameObject.SetActive(true);
        yield return goodEnding.Activate();
    }
}
