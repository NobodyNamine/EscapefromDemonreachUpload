using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    // Start is called before the first frame update
    private Light lightToFlicker;

    private float seed;

    private float xCoord;
    private float yCoord;
    void Start()
    {
        lightToFlicker = GetComponent<Light>();
        xCoord = Random.Range(0, 1);
        yCoord = Random.Range(0, 1);
        seed = transform.parent.position.x;
        Debug.Log(seed);
    }

    // Update is called once per frame
    void Update()
    {
        lightToFlicker.intensity = 1 + (Mathf.PerlinNoise(seed + xCoord, seed + yCoord) / 2);

        xCoord += .015f;
        yCoord += .015f;
    }
}
