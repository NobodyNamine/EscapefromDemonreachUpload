using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Area
{
    SPECIFIC,
    ALL
}
public class Node : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 position;

    public Node next;
    [SerializeField]
    private Area areaContainedIn;
    void Awake()
    {
        position = transform.position;
    }
}
