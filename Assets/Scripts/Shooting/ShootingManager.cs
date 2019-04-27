using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour {

    List<BaseProjectile> _pool;

    [Header("Projectiles pool variables")]
    [Tooltip("Prefab of the projectile to pool")]
    [SerializeField] BaseProjectile _projectile;

    [Tooltip("Amount of projectiles to instantiate at the start")]
    [SerializeField] int _initialAmount;

    [Tooltip("If true, the pool can be expanded to fit more than its initial amount")]
    [SerializeField] bool _canExpand;

    void Start () {

        _pool = new List<BaseProjectile>();

        StartCoroutine(StartPool());
        
    }

    IEnumerator StartPool () {

        for (int i = 0; i < _initialAmount; i++) {

            AddProjectileToPool();

            yield return new WaitForEndOfFrame();

        }        

    }

    public void Shoot (Transform shootingTransform, Vector2 spreadRange) {

        BaseProjectile projectile = null;

        foreach (BaseProjectile p in _pool) {

            if (!p.gameObject.activeSelf) {

                projectile = p;

                break;

            }

        }

        if(!projectile && _canExpand)
            projectile = AddProjectileToPool();

        if(projectile) {

            Vector3 spreadDir = Quaternion.Euler(0, Random.Range(spreadRange.x, spreadRange.y), 0) * shootingTransform.forward;

            projectile.transform.SetPositionAndRotation(shootingTransform.position, Quaternion.LookRotation(spreadDir));

            projectile.gameObject.SetActive(true);

        }

    }

    BaseProjectile AddProjectileToPool() {

        BaseProjectile projectile = Instantiate(_projectile);

        projectile.gameObject.SetActive(false);

        _pool.Add(projectile);

        return projectile;

    }

    // Will eventually be deleted and changed into a Coroutine called by Master
    void Update () {

        foreach (BaseProjectile p in _pool)
            p.transform.position += p.transform.forward * p.Speed * Time.deltaTime;

    }

}
