using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    [SerializeField]
    string triggerTag;
    [SerializeField]
    GameEvent triggerEvent;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == triggerTag)
        {
            triggerEvent.Raise();
        }
    }
}
