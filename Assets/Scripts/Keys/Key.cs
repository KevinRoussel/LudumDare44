using System.Collections;
using UnityEngine;

public class Key : MonoBehaviour {

    [SerializeField] MeshRenderer _renderer;
    [SerializeField] ParticleSystem _collectingKeyEffect;
    [SerializeField] Light _collectingLight;

    Coroutine collecting;
    void OnTriggerEnter (Collider other) {
        
        if(other.CompareTag("Player") && collecting==null) {

            FindObjectOfType<KeysManager>().KeyCollected();
            if(collecting==null) collecting = StartCoroutine(Collecting());
        }

    }

    IEnumerator Collecting()
    {
        _renderer.enabled = false;
        _collectingKeyEffect.Play();

        _collectingLight.enabled = true;
        yield return new WaitForSeconds(0.1f);
        _collectingLight.enabled = false;

        yield return new WaitForSeconds(1f);


        Destroy(gameObject);
        yield break;
    }

}
