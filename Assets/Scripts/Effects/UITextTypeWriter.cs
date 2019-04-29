using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextTypeWriter : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float voiceSpeed;
    Coroutine _currentCoroutine;
    Text txt;
    string story;
    bool running = false;

    [Header("blabla support")]
    [SerializeField] AudioSource _speaker;
    [SerializeField] List<float> _pitches;

    public void ChangeSound(AudioClip voice)
    {
        _speaker.clip = voice;
    }

    public event Action OnSkipDialogue;
    public Coroutine CurrentCoroutine => _currentCoroutine;
    public Coroutine VoiceRoutine { get; private set; }

    void Awake()
    {
        txt = GetComponent<Text>();
    }

    void Update() {
        if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)) {
            txt.text = story;
            StartCoroutine(Finish());
            OnSkipDialogue?.Invoke();

            if(VoiceRoutine!= null)
            {
                StopCoroutine(VoiceRoutine);
                VoiceRoutine = null;
            }

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

        VoiceRoutine = StartCoroutine(VoiceClock());
        IEnumerator VoiceClock()
        {
            while(true)
            {
                _speaker.pitch = _pitches.PickRandom();
                _speaker.Play();
                yield return new WaitForSeconds(1f/voiceSpeed);
            }
        }

        foreach (char c in story)
        {
            if (skip) yield break;

            txt.text += c;
            yield return new WaitForSeconds(1f / speed);
        }

        if (VoiceRoutine != null)
        {
            StopCoroutine(VoiceRoutine);
            VoiceRoutine = null;
        }

        StartCoroutine(Finish());
    }

    IEnumerator Finish() {
        yield return new WaitForSeconds(0.5f);
        running = false;
    }

}
