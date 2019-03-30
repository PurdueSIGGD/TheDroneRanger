using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class MessageTrigger : MonoBehaviour
{
    public string[] characters = { };
    public string[] texts = { };
    public int[] times = { };
    private GameObject UI;
    private DialogueMessage[] messages;

    private void Start()
    {
        UI = GameObject.FindGameObjectWithTag("UI");
        messages = new DialogueMessage[characters.Length];
        for(int i = 0; i < characters.Length; i++)
        {
            messages[i].Character = characters[i];
            messages[i].Message = texts[i];
            messages[i].Time = times[i];
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") == false) return;
        foreach (DialogueMessage mes in messages)
        {
            UI.GetComponent<DialogueBox>().Message(mes);
        }
        Destroy(this.gameObject);
    }
}
