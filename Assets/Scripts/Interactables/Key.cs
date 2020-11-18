using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable
{
    [SerializeField]
    GameManager gameManager;

    float zRotation;
    const float xRotation = 90f;
    const float rotationSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        zRotation = gameObject.transform.rotation.z;

        // If there is no gameManager set, find the gameManager in the scene
        if (gameManager == null)
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (gameManager == null)
            Debug.LogError("No Game Manager in scene");
    }

    // Update is called once per frame
    void Update()
    {
        RotateKey();
    }
    // Rotates the key
    void RotateKey()
    {
        zRotation += rotationSpeed * Time.deltaTime;

        gameObject.transform.rotation =
            Quaternion.Euler(
                xRotation,
                gameObject.transform.rotation.y,
                zRotation);
    }
    protected override void Interaction(Collider other)
    {
        if (!other.GetComponent<Player>())
            return;
        //FMODUnity.RuntimeManager.PlayOneShot("event:/TestSounds/Glitch_1");
        Destroy(gameObject);
        gameManager.CollectKey();
    }
}
