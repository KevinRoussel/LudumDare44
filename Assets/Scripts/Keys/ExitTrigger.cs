using System;
using UnityEngine;

public class ExitTrigger : MonoBehaviour {

    bool _ready;
    public bool Ready { get => _ready; set => _ready = value; }

    public event Action OnGoNextLevel; 

    void OnTriggerEnter (Collider other) {

        if (other.CompareTag("Player") && Ready)
        {
            print("Exit the room");
            OnGoNextLevel?.Invoke();
        }
    }

}
