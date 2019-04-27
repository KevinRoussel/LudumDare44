using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    [SerializeField] ShootingManager _shootingManager;
    [SerializeField] KeysManager _keyManager;

    [Header("Configuration")]
    [SerializeField] Transform _roomRoot;
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] Slider _hpSlider;

    [Header("UI Game")]
    [SerializeField] Animation _gameUI;
    [SerializeField] AnimationClip _gameOpen;
    [SerializeField] AnimationClip _gameClose;

    [Header("UI GameOver")]
    [SerializeField] Animation _gameOverUI;
    [SerializeField] AnimationClip _gameOverOpen;
    [SerializeField] AnimationClip _gameOverClose;

    [Header("Run config")]
    [SerializeField] List<LevelStructure> _mapStructure;
    public List<LevelStructure> MapStructure => _mapStructure;

    [Header("UI")]
    [SerializeField] GameObject GameUI;
    public IEnumerator RunGame()
    {
        _gameUI.gameObject.SetActive(true);
        yield return _gameUI.PlayAndWait(_gameOpen);
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
            _hpSlider.maxValue = currentCharacter.HPMax;
            _hpSlider.value = currentCharacter.HP;

            currentCharacter.OnHPMaxUpdated += (newMax) => _hpSlider.maxValue = newMax;
            currentCharacter.OnTakeDamage += (newHP) =>  _hpSlider.value = newHP;

            //currentCharacter.OnKeyCollected += _keyManager.AddKey();

            foreach (var enemy in currentRoom.Enemies) enemy.Initialization();

            // Pact Room
            // yield return PactRoom();

            // Main Game Loop
            while (currentCharacter.HP > 0)
            {
                _inputManager.ApplyInput(currentCharacter);
                foreach (var el in currentRoom.Enemies) el.Enemy.Movement();
                _shootingManager.UpdateBullets();


                yield return null;
            }

        }

        yield return _gameUI.PlayAndWait(_gameClose);
        _gameUI.gameObject.SetActive(false);

        // GameOver Menu Open
        _gameOverUI.gameObject.SetActive(true);
        yield return _gameOverUI.PlayAndWait(_gameOverOpen);
        yield return new WaitForSeconds(1f);
        yield return _gameOverUI.PlayAndWait(_gameOverClose);
        _gameOverUI.gameObject.SetActive(false);

        yield break;
    }

    IEnumerator PactRoom()
    {
        yield break;
    }
}
