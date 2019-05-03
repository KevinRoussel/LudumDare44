using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyTriggerInsteadButton : MonoBehaviour
{
    // Hard dirty key updating i know #ShameOnMe
    private void Update()
    {
        if (Input.GetKey(KeyCode.P)) GetComponent<Button>().onClick.Invoke();
    }
}
