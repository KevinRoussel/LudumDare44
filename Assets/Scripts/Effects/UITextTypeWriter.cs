﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextTypeWriter : MonoBehaviour
{
    [SerializeField] float speed;
    Coroutine _currentCoroutine;
    Text txt;
    string story;

    public Coroutine CurrentCoroutine => _currentCoroutine;

    void Awake()
    {
        txt = GetComponent<Text>();
    }

    void Update() {
        if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)) {
            StopCoroutine("PlayText");
            txt.text = story;
        }
    }

    public void StartTyping() {
        StopCoroutine("PlayText");

        story = txt.text;
        txt.text = "";

        _currentCoroutine = StartCoroutine("PlayText");
    }

    IEnumerator PlayText()
    {
        foreach (char c in story)
        {
            txt.text += c;
            yield return new WaitForSeconds(1f / speed);
        }
    }
}
