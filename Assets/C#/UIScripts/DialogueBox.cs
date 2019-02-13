using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    private DialogueMessage[] messages = new DialogueMessage[100];
    private int firstEmpty = 0;
    private int currentMessage = 0;
    private float messageTime = -1;
    private bool advance = false;

    [SerializeField]
    private GameObject character;
    [SerializeField]
    private GameObject text;


    public void Message(DialogueMessage mes)
    {
        messages[firstEmpty] = mes;
        firstEmpty++;
        if (firstEmpty >= 100) firstEmpty = 0;
    }

    private void Update()
    {
        if (Input.GetButtonDown("z"))
        {
            advance = true;
        }
        if(firstEmpty != currentMessage)
        {
            if (text.GetComponent<Text>().text.Length >= messages[currentMessage].Message.Length) //Compares the length of text displayed to the remaining text of the message
            {
                if(messages[currentMessage].Time == 0)
                {
                    if (advance)
                    {
                        text.GetComponent<Text>().text = ""; //Removes text
                        currentMessage++;
                        character.GetComponent<RawImage>().uvRect = messages[currentMessage].crop;
                        character.GetComponent<RawImage>().texture = messages[currentMessage].Character; //Updates character image
                    }
                }
                else if(messageTime == -1)
                {
                    messageTime = Time.realtimeSinceStartup;
                }
                else if(Time.realtimeSinceStartup - messageTime >= messages[currentMessage].Time) //Compares the realtime since ending the message with the "time" varible of the message
                {
                    text.GetComponent<Text>().text = "";
                    currentMessage++;
                    character.GetComponent<RawImage>().uvRect = messages[currentMessage].crop;
                    character.GetComponent<RawImage>().texture = messages[currentMessage].Character; //Updates character image
                    messageTime = -1;
                }
            }
            else
            {
                text.GetComponent<Text>().text += messages[currentMessage].Message[text.GetComponent<Text>().text.Length]; //Appends the next character of the message to the dialogue box text
            }
        }
    }
}



public struct DialogueMessage
{
    public Texture Character; //The character to display talking
    public Rect crop;       //The rectangle used to crop the character sprite
    public char[] Message;  //The message that is displayed
    public float Time;      //The time that the message is onscreen after it is finished being displayed - time=0 means the player will advance the message
}