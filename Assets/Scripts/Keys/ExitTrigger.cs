using System;
using UnityEngine;
using UnityEngine.Events;

public class ExitTrigger : MonoBehaviour {

    bool _ready;
    public bool Ready { get => _ready; set { _ready = value; if (_ready) OnExitTriggerReady?.Invoke(); } }

    public event Action OnExitTriggerReady;
    public event Action OnGoNextLevel;

    [SerializeField] UnityEvent ExitTriggerReady;

    private void Start()
    {
        OnExitTriggerReady += () => ExitTriggerReady.Invoke();
    }

    void OnTriggerEnter (Collider other) {

        if (other.CompareTag("Player") && Ready)
        {
            print("Exit the room");
            OnGoNextLevel?.Invoke();
        }
    }

}
