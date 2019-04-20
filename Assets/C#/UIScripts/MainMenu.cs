using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("Scenes/Tutorial");
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit()
    {
        Debug.Log("The game has now closed."); // This is only done for when testing in the unity editor
        Application.Quit(); // This will close the program, when it is built, NOT when it is in the unity editor.
    }
}
