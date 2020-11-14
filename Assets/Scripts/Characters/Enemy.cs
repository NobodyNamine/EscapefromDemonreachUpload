using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    public Area areaPatroling;
    private const int NUM_OF_RAYS = 20;
    private const float CONE_DEGREES = 180;
    private const float VISION_DISTANCE = 50;
    private const float MAX_SPEED = 10;

    protected bool foundPlayer = false;
    protected Vector3 playerPos;

    public float moveSpeed;

    private Rigidbody rigidbody;

    protected NavMeshAgent meshAgent;

    public Node path;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        meshAgent = GetComponent<NavMeshAgent>();
        moveSpeed = 5;
        areaPatroling = Area.ALL;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //If we haven't found the player and
        if(!DetectPlayer() && meshAgent.destination != path.position)
        {
            meshAgent.SetDestination(path.position);
        }

        else if (Vector3.Distance(transform.position, path.position) <= 3)
            PickNextNode();
    }


    protected bool DetectPlayer()
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
                    Debug.Log("Here");
                    //SEEK PLAYER HIT
                    meshAgent.SetDestination(potentialPlayer.transform.position);
                    meshAgent.speed = Mathf.Lerp(meshAgent.speed, MAX_SPEED, .005f);
                    return true;
                }
            }
        }

        return false;
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


    protected void PickNextNode()
    {
        if (areaPatroling == Area.ALL)
            path = path.next;
        else
            path = path.alternateNext;
    }

}
