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

public class Abilities : MonoBehaviour
{
    private ShapeShiftState currentState;

    public bool enableVignette;
    private VignetteState vignetteState;

    private bool NightVisionEnabled = false;
    [SerializeField] private Material nightVisionMaterial;

    private const float RAT_SCALE = .1f;
    private const float HUMAN_SCALE = 1f;
    private const float VIGNETTE_MAX = .55f;
    private const float VIGNETTE_MIN = .6f;
    private const float speedOfTransformation = 0.8f;

    public FirstPersonController FPSRef;
    private float humanWalkSpeed;
    private float humanRunSpeed;
    [SerializeField] private float ratSpeedScale = 0.5f;

    void Start()
    {
        currentState = ShapeShiftState.HUMAN;
        vignetteState = VignetteState.SMALL;
        nightVisionMaterial.SetFloat("_MaskStrength", VIGNETTE_MIN);

        FPSRef = gameObject.GetComponent<FirstPersonController>();
        humanWalkSpeed = FPSRef.m_WalkSpeed;
        humanRunSpeed = FPSRef.m_RunSpeed;
    }

    public void ToggleShapeShiftState()
    {
        if (currentState == ShapeShiftState.RAT)
            currentState = ShapeShiftState.HUMAN;
        else
            currentState = ShapeShiftState.RAT;
    }

    public void DoShapeShift()
    {
        //Scaling up or down if we need to
        if (currentState == ShapeShiftState.RAT && transform.localScale.y >= RAT_SCALE + .01f)
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, RAT_SCALE, 1), speedOfTransformation);
        else if (currentState == ShapeShiftState.HUMAN && transform.localScale.y <= HUMAN_SCALE - .01f)
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, HUMAN_SCALE, 1), speedOfTransformation);

        if (currentState == ShapeShiftState.RAT)
        {
            FPSRef.m_WalkSpeed = humanWalkSpeed * ratSpeedScale;
            FPSRef.m_RunSpeed = humanRunSpeed * ratSpeedScale;
        }
        else
        {
            FPSRef.m_WalkSpeed = humanWalkSpeed;
            FPSRef.m_RunSpeed = humanRunSpeed;
        }
    }

    public void ToggleNightVision()
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

    public void ToggleVignette()
    {
        enableVignette = !enableVignette;
    }

    public void ProcessVisuals()
    {
        if (enableVignette)
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

}
