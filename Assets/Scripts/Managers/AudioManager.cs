using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Fields
    public static AudioManager instance = null;

    //Methods
    //Turns AudioManagers into serial killers who survive scene transistions and murder any other baby GameManagers they encounter
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
        
    }
    private FMOD.Studio.EventInstance ambience = ;
    private FMOD.Studio.EventInstance chaseCloser;
    private FMOD.Studio.EventInstance chaseFarther;

    private void Start()
    {
        
    }
}
