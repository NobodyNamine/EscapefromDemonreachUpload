using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable
{
    float zRotation;

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
        zRotation += 20f * Time.deltaTime;

        gameObject.transform.rotation = Quaternion.Euler(90, 
            gameObject.transform.rotation.y, 
            zRotation);
    }
}
