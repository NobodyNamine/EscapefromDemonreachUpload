using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    // Start is called before the first frame update
    private Light lightToFlicker;

    //Seed used to randomize the flickering
    private float seed;

    //Coordinates on the perlin noise map
    private float xCoord;
    private float yCoord;
    void Start()
    {
        lightToFlicker = GetComponent<Light>();

        //Setting the map to start at a random coordinate
        xCoord = Random.Range(0, 1);
        yCoord = Random.Range(0, 1);


        seed = Vector3.Magnitude(transform.parent.position);
    }

    // Update is called once per frame
    void Update()
    {
        //Changing the intensity of the light using perlin noise
        lightToFlicker.intensity = 1 + (Mathf.PerlinNoise(seed + xCoord, seed + yCoord) / 2);

        //Incrementing our xCoord and yCoord
        xCoord += .015f;
        yCoord += .015f;
    }
}
