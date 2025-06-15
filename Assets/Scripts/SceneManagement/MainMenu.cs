using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string newGameScene;

    public GameObject loadButton;

    public GameObject controlsMenu;

    public string loadGameScene;

    private void Start()
    {
        if(PlayerPrefs.HasKey("Current_Scene"))
        {
            loadButton.SetActive(true);
        }
        else
        {
            loadButton.SetActive(false);
        }
    }

    public void Load()
    {
        SceneManager.LoadScene(loadGameScene);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void OpenControls()
    {
        controlsMenu.SetActive(true);
    }

    public void CloseControls()
    {
        controlsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
