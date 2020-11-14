using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected void OnTriggerEnter(Collider other)
    {
        Interaction(other);
    }

    protected abstract void Interaction(Collider other);
}