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

    /* This class uses a lookup of characters stored in three arrays:
     * characterFaces, characterCrop, characterNames
     * characterNames acts as a lookup for the other two with the functions
     * getTexture(string name) and getCrop(string name)
     * These three arrays must be updated in the script object in the unity editor
     */

    public Texture[] characterFaces;
    public Rect[] characterCrop;
    public string[] characterNames;

    [SerializeField]
    private GameObject characterProfile; //What renders the character
    [SerializeField]
    private GameObject textbox; //What displays the text
    [SerializeField]
    private GameObject messageBox; //The entire dialouge box class

    private void Start()
    {
        textbox.GetComponent<Text>().text = "";
        /*DialogueMessage mes = new DialogueMessage();

        //Debug messages
        mes.Character = "cowboy";
        mes.Message = "Howdy!";
        Message(mes);
        mes.Character = "bullet";
        mes.Message = "Some people think they can outsmart me.\nMaybe ...";
        mes.Time = 1;
        Message(mes);
        mes.Character = "bullet";
        mes.Message = "I have yet to meet man that can outsmart bullet.";
        mes.Time = 2;
        Message(mes);
        */
    }


    public void Message(DialogueMessage mes)
    {
        messages[firstEmpty] = mes;
        if(firstEmpty == currentMessage)
        {
            messageBox.SetActive(true);
            messageTime = -1;
            characterProfile.GetComponent<RawImage>().uvRect = getCrop(messages[currentMessage].Character); //Updates character crop
            characterProfile.GetComponent<RawImage>().texture = getTexture(messages[currentMessage].Character); //Updates character image
        }
        firstEmpty++;
        if (firstEmpty >= 100) firstEmpty = 0;
    }

    public Texture getTexture(string name)
    {
        for (int i = 0; i < characterNames.Length; i++)
        {
            if (characterNames[i].ToLower() == name.ToLower())
            {
                return characterFaces[i];
            }
        }
        return characterFaces[0];
    }

    public Rect getCrop(string name)
    {
        for(int i = 0; i < characterNames.Length; i++)
        {
            if (characterNames[i].ToLower() == name.ToLower())
            {
                return characterCrop[i];
            }
        }
        return characterCrop[0];
    }

    private int timer = 0;
    public int textSpeed = 1;

    private void Update()
    {
        if (messageBox.activeSelf == false)
        {
            return;
        }
        if (Input.GetButtonDown("Submit"))
        {
            advance = true;
        }
        if(firstEmpty != currentMessage)
        {
            if (textbox.GetComponent<Text>().text.Length >= messages[currentMessage].Message.Length) //Compares the length of text displayed to the remaining text of the message
            {
                if(messages[currentMessage].Time == 0)
                {
                    if (advance)
                    {
                        textbox.GetComponent<Text>().text = ""; //Removes text
                        currentMessage++;
                        if (currentMessage >= 100) currentMessage = 0;
                        if(currentMessage == firstEmpty)
                        {
                            messageBox.SetActive(false);
                            return;
                        }
                        characterProfile.GetComponent<RawImage>().uvRect = getCrop(messages[currentMessage].Character); //Updates character crop
                        characterProfile.GetComponent<RawImage>().texture = getTexture(messages[currentMessage].Character); //Updates character image
                        advance = false;
                    }
                }
                else if(messageTime == -1)
                {
                    messageTime = Time.realtimeSinceStartup;
                }
                else if(Time.realtimeSinceStartup - messageTime >= messages[currentMessage].Time) //Compares the realtime since ending the message with the "time" varible of the message
                {
                    textbox.GetComponent<Text>().text = "";
                    currentMessage++;
                    if (currentMessage >= 100) currentMessage = 0;
                    if (currentMessage == firstEmpty)
                    {
                        messageBox.SetActive(false);
                        return;
                    }
                    characterProfile.GetComponent<RawImage>().uvRect = getCrop(messages[currentMessage].Character); //Updates character crop
                    characterProfile.GetComponent<RawImage>().texture = getTexture(messages[currentMessage].Character); //Updates character image
                    messageTime = -1;
                }
            }
            else
            {
                if (advance && messages[currentMessage].Time == 0)
                {
                    textbox.GetComponent<Text>().text = messages[currentMessage].Message;
                    advance = false;
                    return;
                }
                if(timer < textSpeed)
                {
                    timer++;
                    return;
                }
                textbox.GetComponent<Text>().text += messages[currentMessage].Message[textbox.GetComponent<Text>().text.Length]; //Appends the next character of the message to the dialogue box text
                timer = 0;
            }
        }
    }
}



public struct DialogueMessage
{
    public string Character; //The character to display talking, must be present in the character arrays in this script object
    public string Message;  //The message that is displayed
    public float Time;      //The time that the message is onscreen after it is finished being displayed - time=0 means the player will advance the message
}