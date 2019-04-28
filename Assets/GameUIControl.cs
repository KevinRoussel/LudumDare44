using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIControl : MonoBehaviour {

    [Header("UI Elements")]
    [Tooltip("Parent Gameobject for the player's healthbar")]
    [SerializeField] GameObject _playerHealthBar;
    public GameObject PlayerHealthBar => _playerHealthBar;

    [Tooltip("Parent Gameobject for the player's minimap")]
    [SerializeField] GameObject _playerMinimap;    
    public GameObject PlayerMinimap => _playerMinimap;

}
