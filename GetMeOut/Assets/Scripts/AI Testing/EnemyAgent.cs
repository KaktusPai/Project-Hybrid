using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR // To fix build error - Jesse
using UnityEditor;
#endif

//TODO:
//Polish enemy a bit more
//Discuss about functionality and features.

public class EnemyAgent : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject player;
    private Animator enemyAnim;

    //AI
    private EnemyStates currentState = EnemyStates.IDLE;

    //Wandering
    [SerializeField] private List<Transform> waypointsList = new List<Transform>();
    private int currentWaypoint = 0;
    private float roamRadius = 20;
    [SerializeField] private Transform enemyWaypoint;

    private float wanderSpeed = 2.5f;
    
    //Detection
    private float maxDetectionRange = 20;
    private float enemyFov = 45;
    private float enemyWanderFov = 45;

    //Chasing
    private float maxSearchTime = 5f;
    private float defaultPauseTime = 2f;
    private float defaultLostSightTIme = 5f;

    private float searchTimer = 0f;
    private float pauseTime = 0f;
    private float lostSightTime = 0f;

    private float enemtChaseFov = 70;

    private Vector3 lastSeenPosition;

    private float searchRadius = 10f;

    private float chaseSpeed = 5f;

    //Audio
    private AudioSource audioPlayer;
    [SerializeField]private AudioClip monsterRoar;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = this.GetComponent<NavMeshAgent>();
        audioPlayer = this.GetComponent<AudioSource>();
        enemyAnim = this.GetComponent<Animator>();

        currentState = EnemyStates.WANDERING;

        agent.speed = wanderSpeed;
    }

    private void FixedUpdate()
    {
        //Pause time prevents the enemy from moving to a different action while it's in the process of a different one. (for example while playing a screaming animation)
        if (PauseTime()) return;

        Debug.Log(currentState);

        //if the player is spotted, chase the player
        if (IsPlayerSeen())
        {
            lostSightTime = defaultLostSightTIme;

            if (currentState == EnemyStates.CHASING)
            {
                //It's already in a chase
                ChasePlayer();
            }
            else
            {
                //Start a chase
                StartCoroutine(StartChase());
                lastSeenPosition = player.transform.position;
            }
        }
        else
        {
            switch (currentState)
            {
                case EnemyStates.IDLE:
                        ContinueWandering();
                    break;
                case EnemyStates.SEARCHING:
                        SearchForPlayer();
                    break;
                case EnemyStates.CHASING:
                        //The enemy has lost the player out of sight
                        if (LostThePlayer()) //only give up when the player hasnt been seen for a few seconds
                        {
                            LostPlayer();
                        }
                    break;
            }
        }

        //enemy should only wander when the player is not seen and when it's not searching
        if (currentState == EnemyStates.WANDERING && agent.remainingDistance < 1)
        {
            FreeRoam(roamRadius);
        }
    }

    private void LostPlayer()
    {
        enemyAnim.Play("Walking");
        enemyFov = enemyWanderFov;
        currentState = EnemyStates.SEARCHING;
        searchTimer = maxSearchTime;
        SearchTimeLeft();
        agent.SetDestination(lastSeenPosition);
    }

    private void ContinueWandering()
    {
        enemyAnim.Play("Walking");
        currentState = EnemyStates.WANDERING;
        FreeRoam(roamRadius);
    }

    private void SearchForPlayer()
    {
        if (SearchTimeLeft())
        {

            if(agent.remainingDistance < 1)
            {
                FreeRoam(searchRadius);
            }
        }
        else
        {
            currentState = EnemyStates.IDLE;
            enemyAnim.Play("Idle");
            agent.speed = 0;
            pauseTime = defaultPauseTime;
        }
    }

    private void ChasePlayer()
    {
        //Chase the player (set animation, movement speed, etc)
        agent.speed = chaseSpeed;
        agent.SetDestination(player.transform.position);
    }

    private bool LostThePlayer()
    {
        lostSightTime -= Time.deltaTime;
        if (lostSightTime < 0)
        {
            return true;
        }
        return false;
    }

    private IEnumerator StartChase()
    {
        //start animation, sound and timer
        pauseTime = 1.5f;
        agent.speed = 0;
        enemyFov = enemtChaseFov;
        enemyAnim.Play("Roar");
        audioPlayer.PlayOneShot(monsterRoar);
        yield return new WaitForSeconds(1.5f); //animation and sound duration
        agent.speed = chaseSpeed;
        enemyAnim.Play("Running");
        currentState = EnemyStates.CHASING;
    }

    private bool PauseTime()
    {
        pauseTime -= Time.deltaTime;
        if (pauseTime < 0)
        {
            return false;
        }
        agent.speed = 0;
        return true;
    }

    private void StopMovement()
    {
        agent.SetDestination(this.transform.position);
    }

    private void WaypointWander()
    {
        currentWaypoint++;
        agent.SetDestination(waypointsList[currentWaypoint].position);
    }

    private bool SearchTimeLeft()
    {
        searchTimer -= Time.deltaTime;
        if (searchTimer < 0)
        {
            //stop searching (if player was not found)
            return false;
        }

        return true;
    }

    private void FreeRoam(float rad)
    {
        agent.speed = wanderSpeed;

        Vector3 randomDirection = Random.insideUnitSphere * rad;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, rad, 1);
        Vector3 finalPosition = hit.position;
        agent.destination = finalPosition;
    }

    // check to see if a player is within viewing range
    private bool IsPlayerNear()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist < maxDetectionRange) // if true, an Enemy is within range
            return true;
        else return false;
    }

    // check to see if Player is in enemy's current fov
    private bool IsPlayerInEnemyFOV()
    {
        Vector3 targetDir = player.transform.position - transform.position;
        Vector3 forward = transform.forward;
        float angle = Vector3.Angle(targetDir, forward);
        if (angle < enemyFov)
            return true;
        else return false;
    }

    // can the Enemy see the Player or is the view obstructed
    private bool IsPlayerSeen()
    {
        if (!IsPlayerNear() || !IsPlayerInEnemyFOV()) return false;

        Vector3 direction = (player.transform.position - transform.position).normalized;

        RaycastHit hit;

        if ((Physics.Raycast(agent.transform.position, direction, out hit, maxDetectionRange) && hit.collider.tag == "Player"))
        {
            Debug.Log("Sees player");
            return true;
        }
        else
        {
            Debug.Log("Player is behind obstacle");
            return false;
        }
    }
    #if UNITY_EDITOR // To fix build error - Jesse
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Handles.color = Color.yellow;
        Vector3 endPointLeft = this.transform.position + (Quaternion.Euler(0, -enemyFov, 0) * this.transform.transform.forward).normalized * maxDetectionRange;
        Vector3 endPointRight = this.transform.position + (Quaternion.Euler(0, enemyFov, 0) * this.transform.transform.forward).normalized * maxDetectionRange;

        Handles.DrawWireArc(this.transform.position, Vector3.up, Quaternion.Euler(0, -enemyFov, 0) * this.transform.transform.forward, enemyFov * 2, maxDetectionRange);
        Gizmos.DrawLine(this.transform.position, endPointLeft);
        Gizmos.DrawLine(this.transform.position, endPointRight);
    }
    #endif
}
