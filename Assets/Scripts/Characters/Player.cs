using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Character
{
    private Abilities abilityData;
    [SerializeField]
    private Canvas loseCanvas;
    [SerializeField]
    private Canvas UICanvas;
    [SerializeField]
    private Canvas pauseCanvas;
    [SerializeField]
    private Canvas optionsCanvas;
    [SerializeField]
    private Text keysText;
    private GameManager gameManager;
    private Enemy alfred;

    private int nightVisionCooldown;

    private bool spottedAlfred;

    // Start is called before the first frame update
    void Start()
    {
        abilityData = gameObject.GetComponent<Abilities>();
        alfred = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        FindGameManager();
        FindAudioManager();
        //audioManager.PlayAmbience();
        UpdateKeysText();
        EnableFirstPerson(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.CurrentState == gameState.OVER)
            return;
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
        if (GameManager.instance.CurrentState == gameState.GAMEPLAY)
        {
            if (InputData.ActionPressed("pause"))
            {
                UIManager.instance.ForwardCanvas(pauseCanvas);
                GameManager.instance.CurrentState = gameState.PAUSE;

                GetComponent<FirstPersonController>().enabled = false;
                GetComponent<FirstPersonController>().MouseLook.SetCursorLock(false);
            }
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

            if (InputData.ActionPressed("debugEnableVignette"))
            {
                abilityData.ToggleVignette();
            }
        }
        else if(GameManager.instance.CurrentState == gameState.PAUSE)
        {
            if(InputData.ActionPressed("pause"))
            {
                UIManager.instance.BackButton();
                GameManager.instance.CurrentState = gameState.GAMEPLAY;

                GetComponent<FirstPersonController>().enabled = true;
                GetComponent<FirstPersonController>().MouseLook.SetCursorLock(true);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Alfred>() != null || other.GetComponent<Harry>() != null)
        {
            transform.LookAt(new Vector3(other.transform.position.x, other.transform.position.y + 1.0f, other.transform.position.z));
            if(abilityData.currentState == ShapeShiftState.RAT)
            {
                abilityData.ToggleShapeShiftState();
            }
            if (abilityData.Night_Vision_Enabled)
                abilityData.ToggleNightVision();
            UIManager.instance.ForwardCanvas(loseCanvas);
            //Change game state here
            GetComponent<FirstPersonController>().MouseLook.SetCursorLock(false);
            GetComponent<FirstPersonController>().enabled = false;
            audioManager.StopChase();
            Cursor.visible = true;
            UICanvas.gameObject.SetActive(false);
            Destroy(other.gameObject);
            GameManager.instance.CurrentState = gameState.OVER;
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

    private float CheckDistanceToEnemy()
    {
        return Vector3.Distance(gameObject.transform.position, alfred.transform.position);
    }

    private float CheckAngleToEnemy()
    {
        Vector3 targetDir = alfred.transform.position - gameObject.transform.position;
        float angle = Vector3.Angle(targetDir, gameObject.transform.forward);
        return angle;
    }

    public void UpdateKeysText()
    {
        keysText.text = GameManager.instance.KeysAquired.ToString() + "/" + GameManager.instance.KeysRequired.ToString();
    }

    private void PlayStinger()
    {
        if (CheckDistanceToEnemy() < 20)
        { 
            if (CheckAngleToEnemy() < 60)
            {
                if (CheckForWall())
                {
                    if (!spottedAlfred)
                    {
                        audioManager.PlayOneShot("event:/Music/SpottedStinger");
                        spottedAlfred = true;
                    }
                }
            }
        }
    }

    private bool CheckForWall()
    {
        //Shooting out a raycast in that direction
        //RaycastHit hit;
        int layerMask = 1 << 9;
        bool detected = Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), gameObject.transform.forward, CheckDistanceToEnemy(), layerMask);

        if (detected)
        {
            //Debug.Log("Wall Detected");
            return false;
        }

        //Debug.Log("No Wall Detected");
        return true;
    }

    private void AwayFromEnemy() 
    {
        if (CheckDistanceToEnemy() > 20)
        {
            spottedAlfred = false;
        }
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
        //audioManager.StopAmbience();
    }

    public void GoToOptions()
    {
        UIManager.instance.ForwardCanvas(optionsCanvas);
    }

    public void GoBackToPause()
    {
        UIManager.instance.BackButton();
    }

    public void EnableFirstPerson(bool state)
    {
        GetComponent<FirstPersonController>().enabled = state;
        GetComponent<FirstPersonController>().MouseLook.SetCursorLock(state);
    }
}