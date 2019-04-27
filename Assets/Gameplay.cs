using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Gameplay : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] InputManager _inputManager;

    [Header("Configuration")]
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] Transform _playerRoot;


    Character _livePlayer;


    public IEnumerator RunGame()
    {
        // Prepare Run
        _livePlayer = Instantiate(_playerPrefab, _playerRoot).GetComponent<Character>();

        // Main Game Loop
        while (true)
        {
            _inputManager.ApplyInput(_livePlayer);


            yield return null;
        }

        // GameOver Menu

        yield break;

    }
}
