using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShapeShiftState
{
    RAT,
    HUMAN
}

public enum VignetteState
{
    SMALL,
    LARGE
}

public class Player : Character
{
    private ShapeShiftState currentState;

    public bool enableVignette;
    private VignetteState vignetteState;

    private bool NightVisionEnabled = false;
    [SerializeField]
    private Material nightVisionMaterial;

    private const float RAT_SCALE = .1f;
    private const float HUMAN_SCALE = 1f;
    private const float VIGNETTE_MAX = .55f;
    private const float VIGNETTE_MIN = .6f;


    // Start is called before the first frame update
    void Start()
    {
        currentState = ShapeShiftState.HUMAN;
        vignetteState = VignetteState.SMALL;
        nightVisionMaterial.SetFloat("_MaskStrength", VIGNETTE_MIN);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Checking for player input
        ProcessInput();

        //Scaling up or down if we need to
        //later let's [AJ will] move this stuff to abilities and have the speed be a constant in that class and stuff
        const float speedOfTransformation = 0.8f;
        if (currentState == ShapeShiftState.RAT && transform.localScale.y >= RAT_SCALE + .01f)
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, RAT_SCALE, 1), speedOfTransformation);
        else if(currentState == ShapeShiftState.HUMAN && transform.localScale.y <= HUMAN_SCALE - .01f)
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, HUMAN_SCALE, 1), speedOfTransformation);

        if(enableVignette)
        {
            if (vignetteState == VignetteState.LARGE)
            {
                nightVisionMaterial.SetFloat("_MaskStrength", Mathf.Lerp(nightVisionMaterial.GetFloat("_MaskStrength"), VIGNETTE_MAX, .025f));
                if (Mathf.Abs(nightVisionMaterial.GetFloat("_MaskStrength") - VIGNETTE_MAX) <= .01)
                    vignetteState = VignetteState.SMALL;
            }
            else
            {
                nightVisionMaterial.SetFloat("_MaskStrength", Mathf.Lerp(nightVisionMaterial.GetFloat("_MaskStrength"), VIGNETTE_MIN, .025f));
                if (Mathf.Abs(nightVisionMaterial.GetFloat("_MaskStrength") - VIGNETTE_MIN) <= .01)
                    vignetteState = VignetteState.LARGE;
            }
        }
        else
        {
            if (Mathf.Abs(nightVisionMaterial.GetFloat("_MaskStrength") - VIGNETTE_MIN) <= .01)
                nightVisionMaterial.SetFloat("_MaskStrength", Mathf.Lerp(nightVisionMaterial.GetFloat("_MaskStrength"), VIGNETTE_MIN, .025f));
        }
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

        if(Input.GetKeyDown(KeyCode.E))
        {
            enableVignette = !enableVignette;
        }
    }
}