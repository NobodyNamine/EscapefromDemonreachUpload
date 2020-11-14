using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable
{
    float zRotation;
    const float xRotation = 90f;
    const float rotationSpeed = 30f;

    // Start is called before the first frame update
    protected override void Start()
    {
        zRotation = gameObject.transform.rotation.z;
    }

    // Update is called once per frame
    protected override void Update()
    {
        RotateKey();
    }

    protected override void OnInteraction()
    {
        throw new System.NotImplementedException();
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
}
