using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureManager : MonoBehaviour
{
    [SerializeField] Gameplay _gameplay;
    [SerializeField] PactManager _pact;

    IEnumerator Adventure()
    {
        yield break;
    }

#if UNITY_EDITOR
    private void Reset()
    {
        _gameplay = GetComponent<Gameplay>();
        _pact = GetComponent<PactManager>();
    }
#endif

}
