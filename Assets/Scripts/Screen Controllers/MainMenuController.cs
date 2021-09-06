using UnityEngine;

public class MainMenuController : APanelController
{
    [SerializeField]
    GameEvent eventStartGame; 

    public void StartGame()
    {
        eventStartGame.Raise();
    }

    public void ShowCredits()
    {
        Debug.Log("This is supposed to show the credits screen.");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
