using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class controller : MonoBehaviour
{
    public void Loadmainmenu()
    {
        SceneManager.LoadScene("mainmenu");
    }

    public void Loadgameplay()
    {
        SceneManager.LoadScene("gameplay");
    }

    public void Loadgameover()
    {
        SceneManager.LoadScene("gameover");
    }

    public void Loadwin()
    {
        SceneManager.LoadScene("win");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}