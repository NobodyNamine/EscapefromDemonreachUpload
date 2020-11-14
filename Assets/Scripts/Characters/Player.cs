using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    Abilities abilityData;

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
}