using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject optionsCanvas;

    public void StartButton()
    {
        // Load Game Scene
        SceneManager.LoadScene(1);
    }

    public void OptionsButton()
    {
        mainMenuCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void BackButton()
    {
        mainMenuCanvas.SetActive(true);
        optionsCanvas.SetActive(false);
    }
}
