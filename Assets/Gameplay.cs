using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pact
{

}

public class Gameplay : MonoBehaviour
{
    #region InternalTypes

    [Serializable]
    public class LevelStructure
    {
        public Room Room;
        public List<GameObject> EnnemiesPrefab;
        public List<Pact> EnablePacts;  // String will be the new Class representing Bonus
    }

    #endregion

    [Header("Managers")]
    [SerializeField] InputManager _inputManager;
    [SerializeField] KeysManager _keyManager;

    [Header("Configuration")]
    [SerializeField] Transform _roomRoot;
    [SerializeField] GameObject _playerPrefab;

    [Header("Run config")]
    [SerializeField] List<LevelStructure> _mapStructure;
    public List<LevelStructure> MapStructure => _mapStructure;

    [Header("UI")]
    [SerializeField] GameObject GameUI;

    public IEnumerator RunGame()
    {
        List<Pact> _selectedPacts = new List<Pact>();

        foreach(var level in _mapStructure)
        {
            // Spawn Room and Character
            var currentRoom = level.Room;

            var currentCharacter = Instantiate(_playerPrefab, currentRoom.PlayerSpawner)
                .GetComponent<Character>()
                .Initialization();

            currentCharacter.OnKeyCollected += _keyManager.KeyCollected;

            // Activate Game UI
            GameUI.SetActive(true);

            // Pact Room
            // yield return PactRoom();

            // Main Game Loop
            while (true)
            {
                _inputManager.ApplyInput(currentCharacter);




                yield return null;
            }

        }
        // GameOver Menu

        yield break;

    }

    IEnumerator PactRoom()
    {
        yield break;
    }
}
