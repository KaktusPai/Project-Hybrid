using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR 
using UnityEditor;
#endif

public class PlayerAgent : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    public float moveSpeed;
    public Transform target;
    void Start()
    {
        agent.speed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);
    }
}
