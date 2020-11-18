using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum enemyAiState
{
    PATROL,
    ALERTED,
    CHASE,
    INVESTIGATE
}

public class Enemy : Character
{
    public Area areaPatroling;
    private float timer = 0;
    private const int NUM_OF_RAYS = 20;
    private const float CONE_DEGREES = 180;
    private const float VISION_DISTANCE = 20;
    private const float MAX_SPEED = 10;

    protected bool foundPlayer = false;
    protected bool detectPlayerResults = false;
    protected Vector3 playerPos;

    public float moveSpeed;

    private Rigidbody rigidbody;

    protected NavMeshAgent meshAgent;

    private enemyAiState currentState;

    public Node path;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        meshAgent = GetComponent<NavMeshAgent>();
        moveSpeed = 5;
        areaPatroling = Area.ALL;
        currentState = enemyAiState.PATROL;
        meshAgent.SetDestination(path.position);
        //StartCoroutine("Step");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Chasing an invisible player takes priority over everything else so this line gets to be outside the state machine
        //but there probably shouldn't be too much else out here
        detectPlayerResults = DetectPlayer();
        if (detectPlayerResults)
            currentState = enemyAiState.CHASE;
        switch (currentState)
        {
            case enemyAiState.PATROL:
                //The enemy normally walks around from node to node all over the map, looking for evidence
                //The enemy starts in this state and comes back to it after giving up after a certain amount of time spent in INVESTIGATE
                Patrol();
                break;

            /*case enemyAiState.ALERTED:
                //The enemy has gained some evidence, such as hearing the player (while in PATROL or INVESTIGATE) or losing sight of the player (while in CHASE)
                //In this state, the enemy will proceed to node which is closest to the evidence and will switch to INVESTIGATE upon arrival

                //when I arrive at my destination node, find which collection that node beloned to and switch my state to INVESTIGATE
                //That collection of nodes is the target of my investigation
                break;*/

            case enemyAiState.CHASE:
                //The enemy enters this state if they actively see the player during any of the other states
                //They pursue the player

                //If I lose sight of the player then this happens
                if (!detectPlayerResults && Vector3.Distance(transform.position, meshAgent.destination) <= 2)
                {
                    Debug.Log("Here");
                    currentState = enemyAiState.INVESTIGATE;
                    areaPatroling = Area.SPECIFIC;

                    //Find the closest Node to where the player was and look around there
                    FindClosestNode();
                    //probably need some way to mark a node as "evidence" should be whatever node the player was closest too when they were last seen
                }
                break;

            case enemyAiState.INVESTIGATE:
                //The enemy enters this state when they finish ALERTED
                //They will basically do the same thing they do in patrol, but about a small collection of nodes (which the one ALERTED took them too belongs too)
                //instead of all nodes on the map
                Patrol();

                //After a certain amount of time happens, I will give up with the investigation, then switch my state to PATROL
                timer += Time.deltaTime;

                if (timer >= 20)
                {
                    areaPatroling = Area.ALL;
                    currentState = enemyAiState.PATROL;
                    timer = 0;
                }
                break;
        }
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
            Vector3 rayDir = new Vector3(Mathf.Sin((transform.rotation.eulerAngles.y + (((float)i / NUM_OF_RAYS) * CONE_DEGREES) - CONE_DEGREES / 2) * Mathf.Deg2Rad), transform.position.y + 0.2f
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
                    //If we find the player we go into chase mode and adjust our speed
                    meshAgent.SetDestination(potentialPlayer.transform.position);
                    meshAgent.speed = Mathf.Lerp(meshAgent.speed, MAX_SPEED, .005f);
                    currentState = enemyAiState.CHASE;
                    return true;
                }
            }
        }
        return false;
    }
    //Should be called during PATROL and INVESTIGATE
    //The value of the areaPatrolling variable should be the main difference between those states
    protected void Patrol() 
    {
        //Check if we reached our next node
        if (Vector3.Distance(transform.position, path.position) <= 3)
        {
            PickNextNode();
            Debug.Log("Here");
        }
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < NUM_OF_RAYS; i++)
        {
            //Calculating direction of the ray
            Vector3 rayDir = new Vector3(Mathf.Sin((transform.rotation.eulerAngles.y + (((float)i / NUM_OF_RAYS) * CONE_DEGREES) - CONE_DEGREES / 2) * Mathf.Deg2Rad), transform.position.y + .1f
                , Mathf.Cos((transform.rotation.eulerAngles.y + (((float)i / NUM_OF_RAYS) * CONE_DEGREES) - CONE_DEGREES / 2) * Mathf.Deg2Rad));

            Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z), new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z) + rayDir * VISION_DISTANCE);
        }
    }

    protected void PickNextNode()
    {
        if (areaPatroling == Area.ALL)
            path = path.next;
        else
            path = path.alternateNext;

        meshAgent.SetDestination(path.position);
    }

    protected void FindClosestNode()
    {
        //Finds the closest node and sets that to our path
        int index = 0;
        float shortestDistance = Vector3.Distance(GameManager.instance.allNodes[0].position, transform.position);
        float currentDistance = shortestDistance;
        for(int i = 1; i < GameManager.instance.allNodes.Count; i++)
        {
            currentDistance = Vector3.Distance(GameManager.instance.allNodes[i].position, transform.position);
            if(currentDistance < shortestDistance)
            {
                shortestDistance = currentDistance;
                index = i;
            }
        }
        if (areaPatroling == Area.ALL)
            path = GameManager.instance.allNodes[index];
        else
            path = GameManager.instance.allNodes[index];

        meshAgent.SetDestination(path.position);
    }

    IEnumerator Step()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemy/Genius Loci/Footsteps/LociFootstep");

        yield return null;
    }
}
