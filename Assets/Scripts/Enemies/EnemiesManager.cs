using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour {

    private readonly List<BaseEnemy> _enemies = new List<BaseEnemy>();

    public List<BaseEnemy> Enemies => _enemies;

    // Will eventually be deleted and changed into a Coroutine called by Master
    void Update () {

        foreach (BaseEnemy e in _enemies)
            e.Movement();

    }

}
