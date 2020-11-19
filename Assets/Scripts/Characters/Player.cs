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
    private Text nightvisionTimer;
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

        nightVisionCooldown = (int)abilityData.nightVisionTimer;

        if (abilityData.nightVisionTimer > 0)
            nightvisionTimer.text = nightVisionCooldown.ToString();
        else
            nightvisionTimer.text = "0";
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
            UIManager.instance.ForwardCanvas(loseCanvas);
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

    private float CheckDistanceToEnemy()
    {
        return Vector3.Distance(gameObject.transform.position, alfred.transform.position);
    }

    private float CheckAngleToEnemy()
    {
        Vector3 targetDir = alfred.transform.position - gameObject.transform.position;
        float angle = Vector3.Angle(targetDir, gameObject.transform.forward);
        //Debug.Log(angle);
        return angle;
    }

    private void PlayStinger()
    {
        if (CheckDistanceToEnemy() < 40)
        { 
            if (CheckAngleToEnemy() < 20)
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
        if (CheckDistanceToEnemy() > 40)
        {
            spottedAlfred = false;
        }
    }
}