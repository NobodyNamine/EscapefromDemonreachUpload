using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    private GameManager gameManager;

    void Start()
    {
        if (gameManager == null)
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (gameManager == null)
            Debug.LogError("No Game Manager in scene");
    }

    protected override void Interaction(Collider other)
    { 
        if (!other.GetComponent<Player>())
            return;

        if (gameManager.KeysAquired >= gameManager.KeysRequired)
        {
            gameManager.SwitchToScene(0);
        }
    }
}