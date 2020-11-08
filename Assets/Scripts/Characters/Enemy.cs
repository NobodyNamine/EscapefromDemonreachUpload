using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void DetectPlayer()
    {
        int layerMask = 1 << 8;
        layerMask = layerMask | 1 << 9;


    }
}
