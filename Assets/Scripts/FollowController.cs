using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowController : MonoBehaviour
{
    [SerializeField]
    Transform target = null;
    float speed = 2f;
    bool canFollow = true;
    public bool CanFollow {
        set {
            canFollow = value;
            if (canFollow)
            {
                agent.isStopped = false;
            }
            else
            {
                agent.isStopped = true;
            }
        } }
    [SerializeField]
    NavMeshAgent agent = null;

    private void Awake()
    {
        //agent.isStopped = true;
        agent.destination = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 dir = target.position - transform.position;
        //Vector3 destination = dir - dir.normalized;
        if (canFollow)
        {
            if (agent.destination != target.position)
            {
                agent.destination = target.position;
            }
        }
              
    }

    void FollowTarget()
    {
        Vector3 dir = Vector3.Normalize(target.position - transform.position);

        transform.position += dir * speed * Time.deltaTime;
    }
}
