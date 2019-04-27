using UnityEngine;

public class Key : MonoBehaviour {

    void OnTriggerEnter (Collider other) {
        
        if(other.CompareTag("Player")) {

            FindObjectOfType<KeysManager>().KeyCollected();

            Destroy(gameObject);

        }

    }

}
