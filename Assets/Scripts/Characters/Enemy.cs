using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private const int NUM_OF_RAYS = 8;
    private const int CONE_DEGREES = 70;
    private const float VISION_DISTANCE = 10;

    public float moveSpeed;

    private Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        moveSpeed = 10;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        DetectPlayer();
        //Seek(new Vector3(0, 3.5f, 0));
    }


    protected void DetectPlayer()
    {
        //Calculating the player mask for our player
        int layerMask = 1 << 8;
        layerMask = layerMask | 1 << 9;

        //Projecting our rays
        for(int i = 0; i < NUM_OF_RAYS; i++)
        {
            //Calculating direction of the ray
            Vector3 rayDir = new Vector3(Mathf.Cos((transform.rotation.eulerAngles.y + ((i / NUM_OF_RAYS) * CONE_DEGREES)) * Mathf.Deg2Rad), 0, Mathf.Sin((transform.rotation.eulerAngles.y + ((i / NUM_OF_RAYS) * CONE_DEGREES)) * Mathf.Deg2Rad));
            RaycastHit hit;
            Player potentialPlayer;

            //Shooting out a raycast in that direction
            bool detected = Physics.Raycast(transform.position, rayDir, out hit, VISION_DISTANCE, layerMask);
            if(detected)
            {
                if(hit.collider.gameObject.TryGetComponent<Player>(out potentialPlayer))
                {
                    //SEEK PLAYER HIT
                    Seek(potentialPlayer.transform.position);
                }
            }
        }
    }

    protected void Seek(Vector3 position)
    {
        Vector3 forceVector = position - transform.position;

        rigidbody.AddForce(forceVector * moveSpeed);
        Debug.Log("Here");
    }
}
