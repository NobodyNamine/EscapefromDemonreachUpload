using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    private const int NUM_OF_RAYS = 16;
    private const float CONE_DEGREES = 70;
    private const float VISION_DISTANCE = 50;

    protected bool foundPlayer = false;
    protected Vector3 playerPos;

    public float moveSpeed;

    private Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        moveSpeed = 5;        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        DetectPlayer();

        if(foundPlayer)
        {
            Seek(playerPos);
        }
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
            Vector3 rayDir = new Vector3(Mathf.Sin((transform.rotation.eulerAngles.y + (((float)i / NUM_OF_RAYS) * CONE_DEGREES) - CONE_DEGREES / 2) * Mathf.Deg2Rad), 0.1f
                , Mathf.Cos((transform.rotation.eulerAngles.y + (((float)i / NUM_OF_RAYS) * CONE_DEGREES) - CONE_DEGREES / 2) * Mathf.Deg2Rad));
            RaycastHit hit;
            Player potentialPlayer;

            //Shooting out a raycast in that direction
            bool detected = Physics.Raycast(new Vector3(transform.position.x, 0.1f, transform.position.z), rayDir, out hit, VISION_DISTANCE, layerMask);
            if(detected)
            {
                if(hit.collider.gameObject.TryGetComponent<Player>(out potentialPlayer))
                {
                    //SEEK PLAYER HIT
                    playerPos = potentialPlayer.transform.position;
                    foundPlayer = true;
                    //GetComponent<NavMeshAgent>().SetDestination(potentialPlayer.transform.position);
                }
            }
        }
    }

    protected void Seek(Vector3 position)
    {
        Vector3 forceVector = position - transform.position;
        forceVector *= moveSpeed;
        //Vector3.ClampMagnitude(forceVector, .5f);

        rigidbody.AddForce(forceVector);

        transform.LookAt(new Vector3(position.x, transform.position.y, position.z));

    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < NUM_OF_RAYS; i++)
        {
            //Calculating direction of the ray
            Vector3 rayDir = new Vector3(Mathf.Sin((transform.rotation.eulerAngles.y + (((float)i / NUM_OF_RAYS) * CONE_DEGREES) - CONE_DEGREES / 2) * Mathf.Deg2Rad), 0
                , Mathf.Cos((transform.rotation.eulerAngles.y + (((float)i / NUM_OF_RAYS) * CONE_DEGREES) - CONE_DEGREES / 2) * Mathf.Deg2Rad));

            Gizmos.DrawLine(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(transform.position.x, 0, transform.position.z) + rayDir * VISION_DISTANCE);
        }
    }
}
