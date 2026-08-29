using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuutama : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGamePlay()
    {
        SceneManager.LoadScene("pro-pop");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
