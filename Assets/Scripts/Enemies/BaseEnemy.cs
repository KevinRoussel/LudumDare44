using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour {

    NavMeshAgent _navMeshAgent;

    void Start () {
        
        if(!GetComponentInChildren<NavMeshAgent>())
           gameObject.AddComponent<NavMeshAgent>();

        _navMeshAgent = GetComponentInChildren<NavMeshAgent>();

        _navMeshAgent.SetDestination(new Vector3(0, .5f, 0));

    }

}
