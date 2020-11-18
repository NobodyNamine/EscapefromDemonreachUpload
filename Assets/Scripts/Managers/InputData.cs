using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputData
{
    private static Dictionary<string, KeyCode> actionInputs;

    public static void Initialize()
    {
        actionInputs = new Dictionary<string, KeyCode>();

        //Add more to this list to configure the names of actions and what keys they are associated with
        actionInputs.Add("shapeshift", KeyCode.F);
        actionInputs.Add("toggleNightVision", KeyCode.Q);
        actionInputs.Add("debugEnableVignette", KeyCode.E);
        actionInputs.Add("doorEscape", KeyCode.R);
    }

    //Call this method whenever you want to check if the input for an action has been pressed
    public static bool ActionPressed(string action)
    {
        return Input.GetKeyDown(actionInputs[action]);
    }
}
