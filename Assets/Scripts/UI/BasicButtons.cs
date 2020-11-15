using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicButtons : MonoBehaviour
{
    public void StartButton()
    {
        // Load Game Scene
        SceneManager.LoadScene(1);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void BackToMainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
