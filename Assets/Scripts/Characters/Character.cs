using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected AudioManager audioManager;

    protected void FindAudioManager()
    {
        if (audioManager == null)
            audioManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>();

        if (audioManager == null)
            Debug.LogError("No audioManager in scene");
    }
}
