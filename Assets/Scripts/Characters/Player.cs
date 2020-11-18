using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    public Abilities abilityData;
    [SerializeField]
    private Canvas loseCanvas;
    private GameManager gameManager;
    private AudioManager audioManager;
    private Enemy alfred;

    private bool spottedAlfred;

    // Start is called before the first frame update
    void Start()
    {
        abilityData = gameObject.GetComponent<Abilities>();
        alfred = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        FindGameManager();
        FindAudioManager();
    }

    // Update is called once per frame
    void Update()
    {
        //Checking for player input
        ProcessInput();
        abilityData.DoShapeShift();
        abilityData.ProcessVisuals();
        PlayStinger();
        AwayFromEnemy();
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

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Alfred>() != null || other.GetComponent<Harry>() != null)
        {
            //UIManager.instance.ForwardCanvas(loseCanvas);
            //Change game state here
            GetComponent<FirstPersonController>().enabled = false;
            GetComponent<FirstPersonController>().MouseLook.SetCursorLock(false);
            Cursor.visible = true;
        }
    }

    public void RestartGame()
    {
        gameManager.SwitchToScene(0);
    }

    private void FindGameManager()
    {
        if (gameManager == null)
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (gameManager == null)
            Debug.LogError("No Game Manager in scene");
    }

    private void FindAudioManager()
    {
        if (audioManager == null)
            audioManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>();

        if (audioManager == null)
            Debug.LogError("No audioManager in scene");
    }

    private float CheckDistanceToEnemy()
    {
        return Vector3.Distance(gameObject.transform.position, alfred.transform.position);
    }

    private float CheckAngleToEnemy()
    {
        return Vector3.Angle(gameObject.transform.forward, alfred.transform.position);
    }

    private void PlayStinger()
    {
        if (CheckDistanceToEnemy() < 30)
        { 
            if (CheckAngleToEnemy() < 10)
            {
                if (!spottedAlfred)
                {
                    audioManager.PlayOneShot("event:/Music/SpottedStinger");
                    spottedAlfred = true;
                }
            }
        }
    }

    private void AwayFromEnemy() 
    {
        if (CheckDistanceToEnemy() > 60)
        {
            spottedAlfred = false;
        }
    }
}