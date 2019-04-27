using UnityEngine;

public class Room : MonoBehaviour {

    [SerializeField] Transform _playerSpawner;

    public Transform PlayerSpawner => _playerSpawner;

    [SerializeField] Key[] _keys;
    public Key[] Keys => _keys;

    [SerializeField] ExitTrigger _exitTrigger;
    public ExitTrigger ExitTrigger => _exitTrigger;

}
