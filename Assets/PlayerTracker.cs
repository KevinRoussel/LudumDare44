using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    [SerializeField] Vector3 _offset;

    Character _player;

    private void Update()
    {
        if(_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Character>();
        }
         
        if(_player!=null)
        {
            transform.position = _player.transform.position + _offset;
            transform.LookAt(_player.transform);
        }

    }


}
