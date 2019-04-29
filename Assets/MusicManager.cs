using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] Master _master;
    [SerializeField] Gameplay _gameplay;

    [Header("Sources")]
    [SerializeField] AudioSource _menuMusic;
    [SerializeField] AudioSource _fightMusic;
    [SerializeField] AudioSource _pactMusic;
    [SerializeField] AudioSource _endingMusic;

    void StopAll()
    {
        _menuMusic.Stop();
        _fightMusic.Stop();
        _pactMusic.Stop();
        _endingMusic.Stop();
    }

    private void Awake()
    {
        _master.OnMenuStart += () =>
        {
            StopAll();
            _menuMusic.Play();
        };
        _gameplay.OnPactStart += () =>
        {
            StopAll();
            _pactMusic.Play();
        };
        _gameplay.OnMapStart += () =>
        {
            StopAll();
            _fightMusic.Play();
        };
        _gameplay.OnEnding += () =>
        {
            StopAll();
            _endingMusic.Play();
        };
    }


}
