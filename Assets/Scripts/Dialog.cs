using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    [SerializeField] Text name;
    [SerializeField] Text dialogText;

    [SerializeField] UITextTypeWriter typeWriter;

    public void ChangeName(string name) {
        this.name.text = name;
    }

    public void ChangeDialogText(string dialogText) {
        this.dialogText.text = dialogText;
        this.Reset();
    }

    public void Reset() {
        if (this.gameObject.activeInHierarchy) {
            this.typeWriter?.StartTyping();
        }
    }

    public void Toggle() {
        if (!this.gameObject.activeInHierarchy) {
            this.gameObject.SetActive(true);
            this.Reset();
        } else {
            this.gameObject.SetActive(false);
        }
    }
}