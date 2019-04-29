using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextTypeWriter : MonoBehaviour
{
    [SerializeField] float speed;
    Coroutine _currentCoroutine;
    Text txt;
    string story;

    public event Action OnSkipDialogue;
    public Coroutine CurrentCoroutine => _currentCoroutine;

    void Awake()
    {
        txt = GetComponent<Text>();
    }

    void Update() {
        if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)) {
            txt.text = story;
            OnSkipDialogue?.Invoke();
        }
    }

    public void StartTyping() {
        StopCoroutine("PlayText");

        story = txt.text;
        txt.text = "";

        _currentCoroutine = StartCoroutine(PlayText());
    }

    IEnumerator PlayText()
    {
        bool skip = false;
        OnSkipDialogue += () => skip = true;
        foreach (char c in story)
        {
            if (skip) yield break;

            txt.text += c;
            yield return new WaitForSeconds(1f / speed);
        }
    }

}
