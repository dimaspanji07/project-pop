using UnityEngine;

public class PopupQuit : MonoBehaviour
{
    public GameObject popup;

    public void OpenPopup()
    {
        popup.SetActive(true);
    }

    public void ClosePopup()
    {
        popup.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
