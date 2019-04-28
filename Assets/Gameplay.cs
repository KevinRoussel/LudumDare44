using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[Serializable]
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
        public List<PactDialog> Pacts;
    }

    [Serializable]
    public class PactDialog
    {
        public string CharacterName;
        public string DialogText;
        public Pact PactToApply;
        public Sprite _characterImage;
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

    [Header("Pact UI")]
    [SerializeField] Animation _pactUI;
    [SerializeField] AnimationClip _pactUIOpen;
    [SerializeField] AnimationClip _pactUIClose;
    [SerializeField] Transform _demonSelection;
    [SerializeField] Dialog _dialog;
    [SerializeField] Dialog _pactSign;
    [SerializeField] Button _clickTrigger;

    [SerializeField] Button _pactSignOK;
    [SerializeField] Button _pactSignCancel;

    [SerializeField] Image _characterImage;

    [Header("UI GameOver")]
    [SerializeField] Animation _gameOverUI;
    [SerializeField] AnimationClip _gameOverOpen;
    [SerializeField] AnimationClip _gameOverClose;

    [Header("Run config")]
    [SerializeField] List<LevelStructure> _mapStructure;
    public List<LevelStructure> MapStructure => _mapStructure;

    [Header("UI")]
    [SerializeField] GameObject GameUI;

    int _selectedDemon;

    public void SelectDeamon(int idx)
    {
        _selectedDemon = idx;
    }

    public IEnumerator RunGame()
    {
        _gameUI.gameObject.SetActive(true);
        List<Pact> _selectedPacts = new List<Pact>();

        foreach(var level in _mapStructure)
        {
            // Pact
            yield return PactRoom((newPact) => _selectedPacts.Add(newPact));
            IEnumerator PactRoom(Action<Pact> onPactSelected)
            {
                _pactUI.gameObject.SetActive(true);
                yield return _pactUI.PlayAndWait(_pactUIOpen);

                bool _pactCancel = false;

                _demonSelection.gameObject.SetActive(false);
                _dialog.gameObject.SetActive(false);
                _clickTrigger.gameObject.SetActive(false);
                _pactSign.gameObject.SetActive(false);

                do
                {
                    // Select Deamon
                    _pactCancel = false;
                    _demonSelection.gameObject.SetActive(true);
                    _selectedDemon = -1;
                    yield return new WaitWhile(() => _selectedDemon == -1);

                    // Wait dialogue completion
                    _dialog.gameObject.SetActive(true);
                    _dialog.ChangeName(level.Pacts[_selectedDemon].CharacterName)
                        .ChangeDialogText(level.Pacts[_selectedDemon].DialogText);
                    yield return _dialog.TypeWriter.CurrentCoroutine;

                    // Wait click
                    _clickTrigger.gameObject.SetActive(true);
                    bool isDone = false;
                    _clickTrigger.onClick.AddListener(() => isDone = true);
                    yield return new WaitWhile(() => !isDone);

                    _demonSelection.gameObject.SetActive(false);
                    _dialog.gameObject.SetActive(false);
                    _clickTrigger.onClick.RemoveAllListeners();
                    _clickTrigger.gameObject.SetActive(false);

                    // Show Contract
                    _characterImage.sprite = level.Pacts[_selectedDemon]._characterImage;
                    bool _pactOK = false;
                    _pactSign.gameObject.SetActive(true);
                    _pactSignCancel.onClick.AddListener(() => _pactCancel = true);
                    _pactSignOK.onClick.AddListener(() => _pactOK = true);
                    yield return new WaitWhile(() => !_pactCancel && !_pactOK);
                    _pactSign.gameObject.SetActive(false);

                } while (_pactCancel);

                onPactSelected?.Invoke(level.Pacts[_selectedDemon].PactToApply);

                yield return _pactUI.PlayAndWait(_pactUIClose);
                yield break;
            }

            // Spawn Room and Character
            var currentRoom = level.Room;
            var currentCharacter = Instantiate(_playerPrefab, currentRoom.PlayerSpawner)
                .GetComponent<Character>()
                .Initialization();

            GameUI.SetActive(true);
            yield return _gameUI.PlayAndWait(_gameOpen);

            // Activate Game UI
            _hpSlider.maxValue = currentCharacter.HPMax;
            _hpSlider.value = currentCharacter.HP;

            currentCharacter.OnHPMaxUpdated += (newMax) => _hpSlider.maxValue = newMax;
            currentCharacter.OnTakeDamage += (newHP) =>  _hpSlider.value = newHP;

            foreach (var enemy in currentRoom.Enemies) enemy.Initialization();

            // Main Game Loop
            while (currentCharacter.HP > 0)
            {
                //currentCharacter.OnKeyCollected += _keyManager.AddKey();

                _inputManager.ApplyInput(currentCharacter);
                foreach (var el in currentRoom.Enemies) el.Enemy.Tick();
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

    
}
