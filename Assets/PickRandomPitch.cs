using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickRandomPitch : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] List<float> pitches;

    public void ChangePitch()
    {
        source.pitch = pitches.PickRandom();
    }
}
