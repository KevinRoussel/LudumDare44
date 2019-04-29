using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    [SerializeField] Transform _playerSpawner;
    [SerializeField] Transform _enemiesRoot;
    [SerializeField] ExitTrigger _exitTrigger;
    [SerializeField] Key[] _keys;

    public Transform PlayerSpawner => _playerSpawner;
    public IEnumerable<Character> Enemies => _enemiesRoot.GetComponentsInChildren<Character>();
    public Key[] Keys => _keys;
    public ExitTrigger ExitTrigger => _exitTrigger;

}
