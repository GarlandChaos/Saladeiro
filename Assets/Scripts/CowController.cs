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
    CowState stateToNotChangeDirectly = CowState.Idle;
    CowState State
    {
        get { return stateToNotChangeDirectly; }

        set
        {
            stateToNotChangeDirectly = value;

            if (stateToNotChangeDirectly == CowState.Idle || stateToNotChangeDirectly == CowState.Eating)
            {
                followController.CanFollow = false;
            }
            else
            {
                followController.CanFollow = true;
            }
        }
    }

    private void Awake()
    {
        SetColorPropertySettings(colorSettings);
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
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward, Color.white);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1.5f, grasslayerMask))
        {
            if(State != CowState.Eating)
            {
                State = CowState.Eating;
                currentGrass = hit.collider.gameObject;
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

    void DetectRoom() //modificar para lógica de detectar sala
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward, Color.white);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1.5f, roomlayerMask))
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

    void DestroyGrassAndResumeFollow()
    {
        if (currentGrass != null)
        {
            Destroy(currentGrass);
            currentGrass = null;
        }
        State = CowState.Following;
    }

    void SetColorPropertySettings(ColorPropertySettings colorPropertySettings)
    {
        GetComponent<Renderer>().sharedMaterial = colorPropertySettings._Material;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        SetColorPropertySettings(colorSettings);
    }
#endif


}
