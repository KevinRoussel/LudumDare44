using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    [SerializeField] Text name;
    [SerializeField] Text dialogText;

    [SerializeField] UITextTypeWriter typeWriter;

    public void ChangeDialog(string newName, string newDialogText) {
        this.ChangeName(newName);
        this.ChangeDialogText(newDialogText);
    }

    public void ChangeName(string name) {
        this.name.text = name;
    }

    public void ChangeDialogText(string dialogText) {
        this.dialogText.text = dialogText;
    }

    public void Toggle() {
        if (!this.gameObject.activeInHierarchy) {
            this.gameObject.SetActive(true);
            this.typeWriter?.StartTyping();
        } else {
            this.gameObject.SetActive(false);
        }
    }
}