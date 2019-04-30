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
        public string ContractPunchlineBonus;
        public string ContractPunchlineMalus;
        public Pact PactToApply;
        public Sprite[] _characterImage;
        public Sprite[] _pactIcon;
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
    [SerializeField] Image[] _selectedPactsIcon;

    [Header("Pact UI")]
    [SerializeField] Animation _pactUI;
    [SerializeField] AnimationClip _pactUIOpen;
    [SerializeField] AnimationClip _pactUIClose;
    [SerializeField] Transform _demonSelection;
    [SerializeField] Dialog _dialog;
    [SerializeField] Dialog _pactSign;
    [SerializeField] Button _clickTrigger;
    [SerializeField] Text _contractPunchLineMalus;
    [SerializeField] Text _contractPunchLineBonus;

    [SerializeField] Image RedCharacter;
    [SerializeField] Image GreenCharacter;
    [SerializeField] Image BlueCharacter;

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
        var idx = -1;
        foreach(var level in _mapStructure)
        {
            idx++;

            OnNextLevel?.Invoke();

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
                    _demonSelection.gameObject.SetActive(true);

                    // Prepare Deamon image
                    RedCharacter.transform.GetChild(0).GetComponent<Image>().sprite = level.Pacts[0]._characterImage[0];
                    RedCharacter.transform.GetChild(1).GetComponent<Image>().sprite = level.Pacts[0]._characterImage[1];
                    RedCharacter.transform.GetChild(2).GetComponent<Image>().sprite = level.Pacts[0]._characterImage[2];
                    RedCharacter.transform.GetChild(3).GetComponent<Image>().sprite = level.Pacts[0]._characterImage[3];

                    GreenCharacter.transform.GetChild(0).GetComponent<Image>().sprite = level.Pacts[1]._characterImage[0];
                    GreenCharacter.transform.GetChild(1).GetComponent<Image>().sprite = level.Pacts[1]._characterImage[1];
                    GreenCharacter.transform.GetChild(2).GetComponent<Image>().sprite = level.Pacts[1]._characterImage[2];
                    GreenCharacter.transform.GetChild(3).GetComponent<Image>().sprite = level.Pacts[1]._characterImage[3];

                    BlueCharacter.transform.GetChild(0).GetComponent<Image>().sprite = level.Pacts[2]._characterImage[0];
                    BlueCharacter.transform.GetChild(1).GetComponent<Image>().sprite = level.Pacts[2]._characterImage[1];
                    BlueCharacter.transform.GetChild(2).GetComponent<Image>().sprite = level.Pacts[2]._characterImage[2];
                    BlueCharacter.transform.GetChild(3).GetComponent<Image>().sprite = level.Pacts[2]._characterImage[3];

                    RedCharacter.GetComponent<Animation>().Play("_RunPortrait");
                    GreenCharacter.GetComponent<Animation>().Play("_RunPortrait");
                    BlueCharacter.GetComponent<Animation>().Play("_RunPortrait");

                    // Select Demon
                    _pactCancel = false;
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
                    

                    bool _pactOK = false;
                    _characterImage.transform.GetChild(0).GetComponent<Image>().sprite = level.Pacts[_selectedDemon]._characterImage[0];
                    _characterImage.transform.GetChild(1).GetComponent<Image>().sprite = level.Pacts[_selectedDemon]._characterImage[1];
                    _characterImage.transform.GetChild(2).GetComponent<Image>().sprite = level.Pacts[_selectedDemon]._characterImage[2];
                    _characterImage.transform.GetChild(3).GetComponent<Image>().sprite = level.Pacts[_selectedDemon]._characterImage[3];

                    _contractPunchLineMalus.text = level.Pacts[_selectedDemon].ContractPunchlineMalus ?? " ";
                    _contractPunchLineBonus.text = level.Pacts[_selectedDemon].ContractPunchlineBonus ?? " ";

                    _pactSign.gameObject.SetActive(true);
                    _pactSignCancel.onClick.AddListener(() => _pactCancel = true);
                    _pactSignOK.onClick.AddListener(() => _pactOK = true);
                    yield return new WaitWhile(() => !_pactCancel && !_pactOK);
                    yield return null;
                    yield return null;
                    _pactSign.gameObject.SetActive(false);

                } while (_pactCancel);

                onPactSelected?.Invoke(level.Pacts[_selectedDemon].PactToApply);

                _selectedPactsIcon[idx].transform.GetChild(0).GetComponent<Image>().sprite = level.Pacts[_selectedDemon]._pactIcon[0];
                _selectedPactsIcon[idx].transform.GetChild(1).GetComponent<Image>().sprite = level.Pacts[_selectedDemon]._pactIcon[1];
                _selectedPactsIcon[idx].transform.GetChild(2).GetComponent<Image>().sprite = level.Pacts[_selectedDemon]._pactIcon[2];
                _selectedPactsIcon[idx].transform.GetChild(3).GetComponent<Image>().sprite = level.Pacts[_selectedDemon]._pactIcon[3];
                _selectedPactsIcon[idx].transform.GetChild(4).GetComponent<Image>().sprite = level.Pacts[_selectedDemon]._pactIcon[4];

                _pactUI.Play(_pactUIClose.name);
                switch(_selectedDemon)
                {
                    case 0:
                        GameObject.FindObjectOfType<EndingDecision>().AddEvilPoint(7);
                        break;
                    case 1:
                        GameObject.FindObjectOfType<EndingDecision>().AddEvilPoint(4);
                        break;
                    case 2:
                        GameObject.FindObjectOfType<EndingDecision>().AddEvilPoint(0);
                        break;
                }

                yield break;
            }

            OnMapStart?.Invoke();
            if (level == _mapStructure.Last()) OnFinalFight?.Invoke();

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

                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        yield break;
    }

    
}
