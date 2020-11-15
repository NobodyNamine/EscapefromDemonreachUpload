using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    public Abilities abilityData;
    [SerializeField]
    private Canvas loseCanvas;

    // Start is called before the first frame update
    void Start()
    {
        abilityData = gameObject.GetComponent<Abilities>();
    }

    // Update is called once per frame
    void Update()
    {
        //Checking for player input
        ProcessInput();
        abilityData.DoShapeShift();
        abilityData.ProcessVisuals();
    }

    //Method used to check for input from the player
    private void ProcessInput()
    {
        //Toggling our shapeshift
        if (InputData.ActionPressed("shapeshift"))
        {
            abilityData.ToggleShapeShiftState();
        }
        //Toggling Nightvision
        if (InputData.ActionPressed("toggleNightVision"))
        {
            abilityData.ToggleNightVision();
        }

        if(InputData.ActionPressed("debugEnableVignette"))
        {
            abilityData.ToggleVignette();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Alfred>() || other.GetComponent<Harry>())
        {
            UIManager.ForwardCanvas(loseCanvas);
            //Change game state here
            GetComponent<FirstPersonController>().enabled = false;
            GetComponent<FirstPersonController>().MouseLook.SetCursorLock(false);
            Cursor.visible = true;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}