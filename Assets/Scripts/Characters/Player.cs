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

    // Start is called before the first frame update
    void Start()
    {
        abilityData = gameObject.GetComponent<Abilities>();
        FindGameManager();
    }

    // Update is called once per frame
    void Update()
    {
        //Checking for player input
        ProcessInput();
        abilityData.DoShapeShift();
        abilityData.ProcessVisuals();
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
            //UIManager.ForwardCanvas(loseCanvas);
            ////Change game state here
            //GetComponent<FirstPersonController>().enabled = false;
            //GetComponent<FirstPersonController>().MouseLook.SetCursorLock(false);
            Cursor.visible = true;

            gameManager.SwitchToScene(0);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void FindGameManager()
    {
        if (gameManager == null)
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (gameManager == null)
            Debug.LogError("No Game Manager in scene");
    }
}