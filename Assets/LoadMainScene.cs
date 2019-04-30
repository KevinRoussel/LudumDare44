using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainScene : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(WaitAndLoad());
        IEnumerator WaitAndLoad()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(0);
        }
    }
}
