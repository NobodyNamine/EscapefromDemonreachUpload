using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Area
{
    TOP, 
    LEFT,
    RIGHT,
    ALL
}
public class Node : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 position;

    public Node next;
    public Node alternateNext;
    [SerializeField]
    private Area areaContainedIn;
    void Start()
    {
        position = transform.position;
    }
}
