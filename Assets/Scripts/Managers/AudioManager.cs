using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Fields
    public static AudioManager instance = null;

    private FMOD.Studio.EventInstance ambience;
    private FMOD.Studio.EventInstance chaseCloser;
    private FMOD.Studio.EventInstance chaseFarther;

    //Methods
    //Turns AudioManagers into serial killers who survive scene transistions and murder any other baby AudioManagers they encounter
    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ambience = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience/EFD_Ambience");
        chaseCloser = FMODUnity.RuntimeManager.CreateInstance("event:/Music/ChaseCloser");
        chaseFarther = FMODUnity.RuntimeManager.CreateInstance("event:/Music/ChaseFarther");

        //chaseCloser.start();
    }

    public void PlayChase()
    {
        chaseCloser.start();
    }

    public void StopChase()
    {
        chaseCloser.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void PlayAmbience()
    {
        ambience.start();
    }

    public void StopAmbience()
    {
        ambience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void PlayOneShot(string FMODfilePath)
    {
        FMODUnity.RuntimeManager.PlayOneShot(FMODfilePath);
    }
}
