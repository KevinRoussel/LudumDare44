using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{
    Coroutine _game;

    void Awake()
    {
        StartCoroutine(Game());
    }

    IEnumerator Game()
    {

        yield break;
    }


}
