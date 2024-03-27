 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour {

    GameObject[] goalLocations;
    NavMeshAgent agent;
    Animator anim;
    float speedMult;
    float detectionRadius = 5;
    float fleeRadius = 10;

    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        goalLocations = GameObject.FindGameObjectsWithTag("goal");
        int i = Random.Range(0, goalLocations.Length);
        agent.SetDestination(goalLocations[i].transform.position);
        anim = this.GetComponent<Animator>();
        anim.SetTrigger("isWalking");
        anim.SetFloat("wOffset", Random.Range(0.0f, 1.0f));
        float sm = Random.Range(0.5f, 2.0f);
        anim.SetFloat("speedMult", sm);
        agent.speed *= sm;
    }

    void ResetAgent()
    {
        speedMult = Random.Range(0.5f, 2);
        anim.SetFloat("speedMult", speedMult);
        agent.speed *= speedMult;
        anim.SetTrigger("isWalking");
        agent.ResetPath();
    }

    public void DetectNewObstacle(Vector3 position)
    {
        Vector3 fleeDirection = (this.transform.position - position).normalized;
        Vector3 newgoal = this.transform.position + fleeDirection * fleeRadius;

        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(newgoal, path);

        if (path.status != NavMeshPathStatus.PathInvalid)
        {
            agent.SetDestination(path.corners[path.corners.Length - 1]);
            anim.SetTrigger("isRunning");
            agent.speed = 10;
            agent.angularSpeed = 500;
        }
    }

    void Update() 
    {

        if (agent.remainingDistance < 1.0f) 
        {
            ResetAgent();
            int i = Random.Range(0, goalLocations.Length);
            agent.SetDestination(goalLocations[i].transform.position);
        }
    }
}