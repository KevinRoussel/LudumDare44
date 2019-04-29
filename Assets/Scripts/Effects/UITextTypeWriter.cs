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
    bool running = false;

    public event Action OnSkipDialogue;
    public Coroutine CurrentCoroutine => _currentCoroutine;

    void Awake()
    {
        txt = GetComponent<Text>();
    }

    void Update() {
        if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)) {
            txt.text = story;
            StartCoroutine(Finish());
            OnSkipDialogue?.Invoke();
        }
    }

    public void StartTyping() {
        StopCoroutine("PlayText");

        story = txt.text;
        txt.text = "";
        running = true;

        _currentCoroutine = StartCoroutine(PlayText());
    }

    public bool Running() => running;

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
        StartCoroutine(Finish());
    }

    IEnumerator Finish() {
        yield return null;
        running = false;
    }

}
