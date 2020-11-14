using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputData
{
    private static Dictionary<string, KeyCode> actionInputs;

    public static void Initialize()
    {
        actionInputs = new Dictionary<string, KeyCode>();

        actionInputs.Add("shapeshift", KeyCode.F);
        actionInputs.Add("toggleNightVision", KeyCode.Q);
        actionInputs.Add("debugEnableVignette", KeyCode.E);
    }

    public static bool actionPressed(string action)
    {
        return Input.GetKeyDown(actionInputs[action]);
    }
}
