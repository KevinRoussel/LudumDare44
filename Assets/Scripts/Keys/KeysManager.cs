using TMPro;
using UnityEngine;

public class KeysManager : MonoBehaviour {

    [SerializeField] Gameplay _gameplay;

    [SerializeField] AudioSource _allKeysSound;
    Room CurrentRoom { get { return _gameplay.CurrentRoom; } }

    [Header("UI")]
    [SerializeField] TextMeshProUGUI _keysText;

    int _currentRoom, _keysCollected;

    void Awake () {
        _gameplay.OnNextLevel += () => { SetKeysText(0); _keysCollected = 0; };
        SetKeysText(0);

    }

    public void KeyCollected() {

        _keysCollected++;
        SetKeysText(_keysCollected);
        CurrentRoom.ExitTrigger.Ready = (_keysCollected == CurrentRoom.Keys.Length);
        if (CurrentRoom.ExitTrigger.Ready)
            _allKeysSound.Play();
    }

    void SetKeysText(int collected) {

        _keysText.text = collected + " / " + CurrentRoom?.Keys.Length;

    }

}
