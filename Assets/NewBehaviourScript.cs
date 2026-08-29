using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEgine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu")
    }

    public void LoadGameplay()
    {
        SceneManager.LoadScene("pro-pop")
    }

    public void QuitGame()
    {
        Apllication.Quit();
        Debug.Log("Quit")
    }
}
