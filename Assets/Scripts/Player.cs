using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (transform.localScale.x == 1)
                transform.localScale = new Vector3(.25f, .25f, .25f);
            else
                transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
