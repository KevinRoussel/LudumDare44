using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{
    #region InternalTypes

    [Serializable]
    public class LevelStructure
    {
        public string Name;
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
        public AudioClip _voice;
    }

    #endregion

    [Header("Managers")]
    [SerializeField] InputManager _inputManager;
    [SerializeField] ShootingManager _shootingManager;
    [SerializeField] KeysManager _keyManager;
    [SerializeField] GameUIControl _gameUIControl;

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
    [SerializeField] Button _returnMenu;

    [Header("Ending")]
    [SerializeField] EndingDecision _endingDecision;
    [SerializeField] BadEnding _badEnding;
    [SerializeField] GoodEnding _goodEnding;

    [Header("Run config")]
    [SerializeField] List<LevelStructure> _mapStructure;
    public List<LevelStructure> MapStructure => _mapStructure;

    [Header("UI")]
    [SerializeField] GameObject GameUI;

    public Room CurrentRoom { get; set; }

    public event Action OnNextLevel;
    public event Action OnPactStart;
    public event Action OnMapStart;
    public event Action OnEnding;
    public event Action OnFinalFight;

    int _selectedDemon;

    public void SelectDeamon(int idx)
    {
        _selectedDemon = idx;
    }

    public IEnumerator RunGame()
    {

        List<Pact> selectedPacts = new List<Pact>();

        foreach(var level in _mapStructure.Skip(3))
        {
            OnNextLevel?.Invoke();
            if (level == _mapStructure.Last()) OnFinalFight?.Invoke();

            // Pact
            yield return PactRoom((newPact) => selectedPacts.Add(newPact));
            IEnumerator PactRoom(Action<Pact> onPactSelected)
            {
                OnPactStart?.Invoke();
                _pactUI.gameObject.SetActive(true);
                _pactUI.Play(_pactUIOpen.name);

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
                    _dialog.TypeWriter.ChangeSound(level.Pacts[_selectedDemon]._voice);
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
                    yield return null;
                    yield return null;
                    _pactSign.gameObject.SetActive(false);

                } while (_pactCancel);

                onPactSelected?.Invoke(level.Pacts[_selectedDemon].PactToApply);

                _pactUI.Play(_pactUIClose.name);
                yield break;
            }

            OnMapStart?.Invoke();
            _gameUI.gameObject.SetActive(true);

            // Spawn Room and Character
            CurrentRoom = level.Room;
            var currentCharacter = Instantiate(_playerPrefab, CurrentRoom.PlayerSpawner)
                .GetComponent<Character>()
                .Initialization();

            foreach(var pact in selectedPacts)
            {
                pact.Apply(currentCharacter);
                pact.Apply(CurrentRoom);
                pact.Apply(_gameUIControl);
                pact.Apply(_inputManager);
            }

            GameUI.SetActive(true);

            // Activate Game UI
            _hpSlider.maxValue = currentCharacter.HPMax;
            _hpSlider.value = currentCharacter.HP;

            currentCharacter.OnHPMaxUpdated += (newMax) => _hpSlider.maxValue = newMax;
            currentCharacter.OnTakeDamage += (newHP) =>  _hpSlider.value = newHP;

            foreach (var enemy in CurrentRoom.Enemies) enemy.Initialization();

            // Main Game Loop
            bool nextLevel = false;
            CurrentRoom.ExitTrigger.OnGoNextLevel += () => nextLevel = true;

            while (!nextLevel)
            {
                //currentCharacter.OnKeyCollected += _keyManager.AddKey();

                // GameOver
                if (currentCharacter.HP <= 0)
                {
                    // GameOver Menu Open
                    _gameOverUI.gameObject.SetActive(true);
                    yield return _gameOverUI.PlayAndWait(_gameOverOpen);

                    bool _done = false;
                    _returnMenu.onClick.AddListener(() => _done = true);
                    yield return new WaitWhile(()=>!_done);
                    _returnMenu.onClick.RemoveAllListeners();

                    yield return _gameOverUI.PlayAndWait(_gameOverClose);
                    _gameOverUI.gameObject.SetActive(false);

                    _gameUI.gameObject.SetActive(false);
                    SceneManager.LoadScene(0);
                    yield break;
                }
                _inputManager.ApplyInput(currentCharacter);
                foreach (var el in CurrentRoom.Enemies) el.Enemy.Tick();
                _shootingManager.UpdateBullets();

                yield return null;
            }

            Destroy(currentCharacter.gameObject);
        }
        
        // ENDING HERE
        OnEnding?.Invoke();
        yield return _endingDecision.StartEnding();


        yield return _gameUI.PlayAndWait(_gameClose);
        _gameUI.gameObject.SetActive(false);
        yield break;
    }

    
}
