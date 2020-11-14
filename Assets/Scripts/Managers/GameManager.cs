using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private List<Node> allNodes;

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
        InputData.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "StartScene")
        {
            GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");

            for(int i = 0; i < nodes.Length; i++)
            {
                allNodes.Add(nodes[i].GetComponent<Node>());
            }
        }
    }
}
