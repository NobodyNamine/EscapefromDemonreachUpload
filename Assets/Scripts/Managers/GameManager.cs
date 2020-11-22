using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum gameState
{
    MAINMENU,
    GAMEPLAY,
    PAUSE
}

public class GameManager : MonoBehaviour
{
    //Fields
    public static GameManager instance = null;

    public List<Node> allNodes;

    private int keysAquired = 0;
    private const int NUM_OF_KEYS_REQUIRED = 5;

    private gameState currentState;

    private Enemy enemyRef;

    //Properties
    public int KeysAquired { get { return keysAquired; } }
    public int KeysRequired { get { return NUM_OF_KEYS_REQUIRED; } }
    public gameState CurrentState { get { return currentState; } }

    //Methods
    //Turns GameManagers into serial killers who survive scene transistions and murder any other baby GameManagers they encounter
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        InputData.Initialize();

        //set the game state
        currentState = gameState.MAINMENU;
        //having to always launch from the Main Menu during development would be annoying so OnSceneLoaded accounts for that

        enemyRef = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case gameState.MAINMENU:
                //things happen here probably, offload what you can into UiManager helper methods
                break;

            case gameState.GAMEPLAY:
                //things happen here probably, offload what you can into other Manager class helper methods
                if (keysAquired >= NUM_OF_KEYS_REQUIRED)
                    enemyRef.currentState = enemyAiState.CHASE;
                break;

            case gameState.PAUSE:
                //things happen here probably, offload what you can into UiManager helper methods
                break;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //This is only here because always starting on the main menu during development would be annoying
        if (scene.name != "MainMenu")
            currentState = gameState.GAMEPLAY;
        //This part above would not be neccesary in a final build

        switch (currentState)
        {
            case gameState.MAINMENU:
                //idk is there anything we need to do when the Main Menu scene loads but jic
                break;

            case gameState.GAMEPLAY:
                GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");

                for (int i = 0; i < nodes.Length; i++)
                {
                    allNodes.Add(nodes[i].GetComponent<Node>());
                }
                break;
        }
    }

    public void CollectKey()
    {
        keysAquired++;

        if (keysAquired == KeysRequired)
        {
            Debug.Log("ALL KEYS AQUIRED");
        }
    }

    public void SwitchToScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
