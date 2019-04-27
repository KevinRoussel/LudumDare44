using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PactManager : MonoBehaviour
{
    [SerializeField] Gameplay _gameplay;
    [SerializeField] AdventureManager _adventure;

    IEnumerator PactSession()
    {
        yield break;
    }


#if UNITY_EDITOR
    private void Reset()
    {
        _gameplay = GetComponent<Gameplay>();
        _adventure = GetComponent<AdventureManager>();
    }
#endif

}
