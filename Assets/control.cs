using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ngaturin : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu")
    }

    public void LoadGamePlay()
    {
        SceneManager.LoadScene("pro-pop")
    }

    public void QuitGame()
    {
        Application.Quit()
        Debug.Log("Quit")
    }
}