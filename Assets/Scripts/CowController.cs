using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CowState
{
    Idle,
    Following,
    Eating
}

public class CowController : MonoBehaviour, IInteractable
{
    [SerializeField]
    ColorPropertySettings colorSettings = null;
    float radius = 1f;
    [SerializeField]
    LayerMask grasslayerMask;
    [SerializeField]
    LayerMask roomlayerMask;
    [SerializeField]
    FollowController followController = null;
    [SerializeField]
    GameObject currentGrass = null;
    [SerializeField]
    Animator animator = null;
    int WalkHash = Animator.StringToHash("Walk");
    int EatHash = Animator.StringToHash("Eat");
    [SerializeField]
    float detectionRange = 1.5f;
    CowState stateToNotChangeDirectly = CowState.Idle;
    CowState State
    {
        get { return stateToNotChangeDirectly; }

        set
        {
            stateToNotChangeDirectly = value;

            if(stateToNotChangeDirectly == CowState.Following)
            {
                followController.CanFollow = true;
                animator.SetBool(EatHash, false);
                //animator.SetBool(WalkHash, true);
            }
            else
            {
                followController.CanFollow = false;
                animator.SetBool(WalkHash, false);
                if(stateToNotChangeDirectly == CowState.Eating)
                {
                    animator.SetBool(EatHash, true);
                }
            }
        }
    }

    private void Awake()
    {
        //SetColorPropertySettings(colorSettings);
        State = CowState.Idle;
    }

    private void Start()
    {
        CattleMinigameManager.instance.RegisterCow(this);
    }

    private void Update()
    {
        if(State == CowState.Following)
        {
            DetectGrass();
            DetectRoom();
        }
    }

    void DetectGrass()
    {
        Vector3 rayOrigin = transform.position;
        rayOrigin.y += 0.5f;
#if UNITY_EDITOR
        Debug.DrawRay(rayOrigin, transform.forward * detectionRange, Color.white);
        Debug.DrawRay(rayOrigin, Quaternion.AngleAxis(-45, Vector3.up) * transform.forward * detectionRange, Color.white);
        Debug.DrawRay(rayOrigin, Quaternion.AngleAxis(45, Vector3.up) * transform.forward * detectionRange, Color.white);
#endif
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, transform.forward, out hit, detectionRange, grasslayerMask)) //Ray 1
        {
            if(State != CowState.Eating)
            {
                SetStateToEating(hit.collider.gameObject);
            }
        }
        else if (Physics.Raycast(rayOrigin, Quaternion.AngleAxis(-45, Vector3.up) * transform.forward, out hit, detectionRange, grasslayerMask)) //Ray 2
        {
            if (State != CowState.Eating)
            {
                SetStateToEating(hit.collider.gameObject);
            }
        }
        else if (Physics.Raycast(rayOrigin, Quaternion.AngleAxis(45, Vector3.up) * transform.forward, out hit, detectionRange, grasslayerMask)) //Ray 3
        {
            if (State != CowState.Eating)
            {
                SetStateToEating(hit.collider.gameObject);
            }
        }
        else
        {
            if (currentGrass)
            {
                currentGrass = null;
            }
        }
    }

    void SetStateToEating(GameObject grass)
    {
        State = CowState.Eating;
        currentGrass = grass;
        Vector3 distanceFromGrass = Vector3.Normalize(grass.transform.position - transform.position);
        transform.position = grass.transform.position - distanceFromGrass;
        transform.LookAt(currentGrass.transform);
    }

    void DetectRoom()
    {
        Vector3 rayOrigin = transform.position;
        rayOrigin.y += 0.5f;
#if UNITY_EDITOR
        Debug.DrawRay(rayOrigin, transform.forward * detectionRange, Color.white);
#endif
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, transform.forward, out hit, detectionRange, roomlayerMask))
        {
            ColorCode go = hit.collider.gameObject.GetComponentInParent<ColorRoom>().color; //mudar onde pega o componente após usar modelos 3D

            if (colorSettings._ColorCode == go)
            {
                CattleMinigameManager.instance.UnregisterCow(this);
                Destroy(gameObject);
            }
        }
    }

    public void Interact()
    {
        CattleMinigameManager.instance.SetCurrentCow(this);
        if(State == CowState.Eating)
        {
            DestroyGrassAndResumeFollow();
        }
        else
        {
            State = CowState.Following;
        }
    }

    public void Stop()
    {
        State = CowState.Idle;
    }

    public void WaitToResumeFollowing()
    {
        animator.SetBool(WalkHash, false);
    }

    public void ResumeFollowing()
    {
        animator.SetBool(WalkHash, true);
    }

    void DestroyGrassAndResumeFollow()
    {
        if (currentGrass != null)
        {
            Destroy(currentGrass);
            currentGrass = null;
        }
        State = CowState.Following;
    }

//    void SetColorPropertySettings(ColorPropertySettings colorPropertySettings)
//    {
//        GetComponent<Renderer>().sharedMaterial = colorPropertySettings._Material;
//    }

//#if UNITY_EDITOR
//    private void OnValidate()
//    {
//        SetColorPropertySettings(colorSettings);
//    }
//#endif


}
