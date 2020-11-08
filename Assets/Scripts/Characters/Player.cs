using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShapeShiftState
{
    RAT,
    HUMAN
}

public class Player : Character
{
    private ShapeShiftState currentState;

    private bool NightVisionEnabled = false;
    [SerializeField]
    private Material nightVisionMaterial;

    private const float RAT_SCALE = .1f;
    private const float HUMAN_SCALE = 1f;

    // Start is called before the first frame update
    void Start()
    {
        currentState = ShapeShiftState.HUMAN;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Checking for player input
        ProcessInput();

        //Scaling up or down if we need to
        if (currentState == ShapeShiftState.RAT && transform.localScale.y >= RAT_SCALE + .01f)
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, RAT_SCALE, 1), .01f);
        else if(currentState == ShapeShiftState.HUMAN && transform.localScale.y <= HUMAN_SCALE - .01f)
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, HUMAN_SCALE, 1), .01f);
    }

    //Method used to check for input from the player
    void ProcessInput()
    {
        //Toggling our shapeshift
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentState == ShapeShiftState.RAT)
                currentState = ShapeShiftState.HUMAN;
            else
                currentState = ShapeShiftState.RAT;
        }
        //Toggling Nightvision
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (NightVisionEnabled)
            {
                nightVisionMaterial.SetFloat("_Enabled", 0);
            }
            else
            {
                nightVisionMaterial.SetFloat("_Enabled", 1);
            }
            NightVisionEnabled = !NightVisionEnabled;
        }
    }
}