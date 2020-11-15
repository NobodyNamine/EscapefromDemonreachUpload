using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : BasicButtons
{
    public GameObject mainMenuCanvas;
    public GameObject optionsCanvas;

    public void OptionsButton()
    {
        mainMenuCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
    }

    public void BackButton()
    {
        mainMenuCanvas.SetActive(true);
        optionsCanvas.SetActive(false);
    }
}
