using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    //Fields
    public float interactableDistance;
    protected string interactionType; //this might not be needed, and if it is it might be better as an enum

    //Properties
    //public float InteractableDistance { get { return interactableDistance; } }
    public string InteractionType { get { return interactionType; } }

    //Methods
    public virtual bool Interacted()
    {
        return false;
        //TODO: implement default interaction
    }

    protected abstract void OnInteraction();
    // Start is called before the first frame update
    protected virtual void Start()
    {
        interactionType = "noInputWithinDistance";
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(Interacted())
        {
            OnInteraction();
        }
    }
}
