using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

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
                //agent.enabled = true;
                agent.isStopped = false;
                agent.updatePosition = true;
                agent.updateRotation = true;
            }
            else
            {
                agent.isStopped = true;
                agent.updatePosition = false;
                agent.updateRotation = false;
                //agent.enabled = false;
            }
        } }
    [SerializeField]
    NavMeshAgent agent = null;
    bool waitingToFollow = false;
    [SerializeField]
    UnityEvent waitToFollowEvent = null;
    [SerializeField]
    UnityEvent resumeFollowEvent = null;

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

                if (agent.velocity == Vector3.zero)
                {
                    if (!waitingToFollow)
                    {
                        waitToFollowEvent.Invoke();
                        waitingToFollow = true;
                    }
                }
                else if(waitingToFollow)
                {
                    resumeFollowEvent.Invoke();
                    waitingToFollow = false;
                }
            }
        }
              
    }

    void FollowTarget()
    {
        Vector3 dir = Vector3.Normalize(target.position - transform.position);

        transform.position += dir * speed * Time.deltaTime;
    }
}
