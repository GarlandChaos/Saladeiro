using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    bool canInteract = false;
    IInteractable interactable = null;
    [SerializeField]
    LayerMask interactionLayerMask;
    [SerializeField]
    float yOffset = 0f;

    private void Update()
    {
        Vector3 rayPos = transform.position + new Vector3(0f, yOffset, 0f);
        RaycastHit hit;
#if UNITY_EDITOR
        Debug.DrawRay(rayPos, transform.forward, Color.white);
#endif
        if (Physics.Raycast(rayPos, transform.forward, out hit, 1.5f, interactionLayerMask))
        {
            if (!canInteract)
            {
                canInteract = true;
                GameObject go = hit.collider.gameObject;
                interactable = go.GetComponentInParent<IInteractable>();
                float ySize = 0f;
                if (go.GetComponent<MeshRenderer>())
                {
                    ySize = go.GetComponent<MeshRenderer>().bounds.size.y;
                }
                else if (go.GetComponent<SkinnedMeshRenderer>())
                {
                    ySize = go.GetComponent<SkinnedMeshRenderer>().bounds.size.y;
                }
                UIManager.instance._InteractionIndicator.transform.position = go.transform.position + new Vector3(0, ySize, 0f);
                UIManager.instance._InteractionIndicator.SetActive(true);
            }

            UIManager.instance._InteractionIndicator.transform.LookAt(Camera.main.transform);
        }
        else
        {
            if (canInteract)
            {
                canInteract = false;
                interactable = null;
                UIManager.instance._InteractionIndicator.SetActive(false);
            }
        }

        if (canInteract && Input.GetKeyDown(KeyCode.C))
        {
            interactable.Interact();
        }
    }
}
