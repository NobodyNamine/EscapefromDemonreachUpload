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
    private float timer = 0;
    private const int NUM_OF_RAYS = 20;
    private const float CONE_DEGREES = 180;
    private const float VISION_DISTANCE = 30;
    private const float MAX_SPEED = 10;

    protected bool foundPlayer = false;
    protected bool capturedEnemy = false;
    protected bool detectPlayerResults = false;
    protected Vector3 playerPos;

    public float moveSpeed;

    private Rigidbody rigidbody;
    public List<Node> KeyNodes;
    private Node currentKeyNode;

    protected NavMeshAgent meshAgent;

    public enemyAiState currentState;

    public Node path;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        meshAgent = GetComponent<NavMeshAgent>();
        moveSpeed = 5;
        currentState = enemyAiState.PATROL;
        //currentKeyNode = KeyNodes[Random.Range(0, GameManager.instance.KeysRequired)];
        currentKeyNode = KeyNodes[0];
        path = currentKeyNode;
        transform.position = path.position;
        meshAgent.SetDestination(path.position);
        FindAudioManager();
        //StartCoroutine("Step");

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (GameManager.instance.CurrentState == gameState.PAUSE)
        {
            meshAgent.enabled = false;
            return;
        }

        if (meshAgent.enabled == false)
        {
            meshAgent.enabled = true;
            meshAgent.SetDestination(currentKeyNode.position);
        }
        if (capturedEnemy)
            return;
        //Chasing an invisible player takes priority over everything else so this line gets to be outside the state machine
        //but there probably shouldn't be too much else out here
        detectPlayerResults = DetectPlayer();
        if (detectPlayerResults)
        {
            // If the enemy has found the player before entering chase state, start playing the chase music
            if (!foundPlayer)
                audioManager.PlayChase();

            // enemy has found player
            foundPlayer = true;

            currentState = enemyAiState.CHASE;
        }

        switch (currentState)
        {
            case enemyAiState.PATROL:
                //The enemy normally walks around from node to node all over the map, looking for evidence
                //The enemy starts in this state and comes back to it after giving up after a certain amount of time spent in INVESTIGATE
                Patrol();
                break;

            case enemyAiState.CHASE:
                //The enemy enters this state if they actively see the player during any of the other states
                //They pursue the player
                //If I lose sight of the player then this happens
                if (!detectPlayerResults && Vector3.Distance(transform.position, meshAgent.destination) <= 1)
                {
                    // If the enemy had found the player before going into investigate, stop playing hte chase music
                    if (foundPlayer)
                        audioManager.StopChase();

                    // the enemy has not found the player
                    foundPlayer = false;

                    currentState = enemyAiState.INVESTIGATE;
                    currentKeyNode = path;
                    path = FindPath();

                    //Find the closest Node to where the player was and look around there
                    meshAgent.SetDestination(path.transform.position);
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

                if (timer >= 30)
                {
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
            Vector3 rayDir = new Vector3(Mathf.Sin((transform.rotation.eulerAngles.y + (((float)i / NUM_OF_RAYS) * CONE_DEGREES) - CONE_DEGREES / 2) * Mathf.Deg2Rad), 0
                , Mathf.Cos((transform.rotation.eulerAngles.y + (((float)i / NUM_OF_RAYS) * CONE_DEGREES) - CONE_DEGREES / 2) * Mathf.Deg2Rad));
            RaycastHit hit;
            Player potentialPlayer;

            //Shooting out a raycast in that direction
            bool detected = Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), rayDir, out hit, VISION_DISTANCE, layerMask);

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

        for(int i = 0; i < NUM_OF_RAYS; i++)
        {
            //Calculating direction of the ray
            Vector3 rayDir = new Vector3(Mathf.Sin((-transform.rotation.eulerAngles.y + (((float)i / NUM_OF_RAYS) * CONE_DEGREES) - CONE_DEGREES / 2) * Mathf.Deg2Rad), 0
                , Mathf.Cos((-transform.rotation.eulerAngles.y + (((float)i / NUM_OF_RAYS) * CONE_DEGREES) - CONE_DEGREES / 2) * Mathf.Deg2Rad));
            RaycastHit hit;
            Player potentialPlayer;

            //Shooting out a raycast in that direction
            bool detected = Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), rayDir, out hit, VISION_DISTANCE/2, layerMask);

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
        if (Vector3.Distance(transform.position, path.position) <= 3)
        {
            Debug.Log("Here");
            PickNextNode();
        }
    }

    protected void EndInvestigation()
    {
        currentState = enemyAiState.PATROL;
        path = currentKeyNode;
        timer = 0;
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < NUM_OF_RAYS; i++)
        {
            //Calculating direction of the ray
            Vector3 rayDir = new Vector3(Mathf.Sin((transform.rotation.eulerAngles.y + (((float)i / NUM_OF_RAYS) * CONE_DEGREES) - CONE_DEGREES / 2) * Mathf.Deg2Rad), 0
                , Mathf.Cos((transform.rotation.eulerAngles.y + (((float)i / NUM_OF_RAYS) * CONE_DEGREES) - CONE_DEGREES / 2) * Mathf.Deg2Rad));

            Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(transform.position.x, transform.position.y, transform.position.z) + rayDir * VISION_DISTANCE);

            rayDir = new Vector3(Mathf.Sin((-transform.rotation.eulerAngles.y + (((float)i / NUM_OF_RAYS) * CONE_DEGREES) - CONE_DEGREES / 2) * Mathf.Deg2Rad), 0
                , Mathf.Cos((-transform.rotation.eulerAngles.y + (((float)i / NUM_OF_RAYS) * CONE_DEGREES) - CONE_DEGREES / 2) * Mathf.Deg2Rad));

            Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(transform.position.x, transform.position.y, transform.position.z) + rayDir * VISION_DISTANCE/2);
        }
    }

    protected void PickNextNode()
    {
        if (path == currentKeyNode)
            currentKeyNode = path.next;
        if (path.next == null)
        {
            EndInvestigation();
        }
        path = path.next;
        meshAgent.SetDestination(path.position);
    }

    protected Node FindClosestNode()
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

        return GameManager.instance.allNodes[index];
    }
    
    protected Node FindPath()
    {
        List<Node> allNodes = GameManager.instance.allNodes;
        List<Node> nodesToPathTo = new List<Node>();
        int indexOfFurthest = 0;
        float furthestDistance = Vector3.Distance(allNodes[0].position, transform.position);
        float currentDistance = furthestDistance;
        nodesToPathTo.Add(allNodes[0]);
        for(int i = 1; i < allNodes.Count; i++)
        {
            currentDistance = Vector3.Distance(allNodes[i].position, transform.position);
            if (nodesToPathTo.Count < 3)
            {
                nodesToPathTo.Add(allNodes[i]);
                if (currentDistance > furthestDistance)
                {
                    furthestDistance = currentDistance;
                    indexOfFurthest = i;
                }
            }
            else if (currentDistance < furthestDistance)
            {
                nodesToPathTo.RemoveAt(indexOfFurthest);
                nodesToPathTo.Add(allNodes[i]);
                currentDistance = Vector3.Distance(nodesToPathTo[0].position, transform.position);
                indexOfFurthest = 0;
                furthestDistance = currentDistance;
                for(int j = 1; j < nodesToPathTo.Count; j++)
                {
                    currentDistance = Vector3.Distance(nodesToPathTo[j].position, transform.position);
                    if(currentDistance > furthestDistance)
                    {
                        furthestDistance = currentDistance;
                        indexOfFurthest = j;
                    }
                }
            }
        }

        nodesToPathTo[indexOfFurthest].next = nodesToPathTo[(indexOfFurthest + 1) % nodesToPathTo.Count];
        nodesToPathTo[(indexOfFurthest + 1) % nodesToPathTo.Count].next = nodesToPathTo[(indexOfFurthest + 2) % nodesToPathTo.Count];

        return nodesToPathTo[indexOfFurthest];
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            transform.position -= 1.5f * (new Vector3(other.transform.position.x, 0, other.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z));
            transform.LookAt(new Vector3(other.transform.position.x, other.transform.position.y - 3.0f, other.transform.position.z));
            capturedEnemy = true;
            meshAgent.enabled = false;
        }
    }

    //IEnumerator Step()
    //{
    //    FMODUnity.RuntimeManager.PlayOneShot("event:/Enemy/Genius Loci/Footsteps/LociFootstep");

    //    yield return null;
    //}
}
