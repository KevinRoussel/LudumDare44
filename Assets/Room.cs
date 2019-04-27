using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    [SerializeField] Transform _playerSpawner;
    [SerializeField] Transform _enemiesRoot;

    public Transform PlayerSpawner => _playerSpawner;
    public IEnumerable<Character> Enemies => _enemiesRoot.GetComponentsInChildren<Character>();
    [SerializeField] Key[] _keys;
    public Key[] Keys => _keys;
    [SerializeField] ExitTrigger _exitTrigger;
    public ExitTrigger ExitTrigger => _exitTrigger;

}
