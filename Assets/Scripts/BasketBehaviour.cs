using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBehaviour : MonoBehaviour
{
    [SerializeField]
    List<Transform> path = new List<Transform>();
    int pathIndex = 0;
    [SerializeField]
    GameEvent eventCanInteractWithAirCables = null, eventEndAirCablesMinigame = null;
    [SerializeField]
    Camera cam = null;

    private void Awake()
    {
        cam.enabled = false;
    }

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void ActivateCamera()
    {
        cam.enabled = true;
    }

    public void DeactivateCamera()
    {
        cam.enabled = false;
    }

    public void MoveToNextPoint()
    {
        if(pathIndex < path.Count - 1)
        {
            Debug.Log("Started coroutine");
            StartCoroutine(MoveToNextPointCoroutine());
        }
    }

    IEnumerator MoveToNextPointCoroutine()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        float timer = 0f;
        float animationTime = 2f;
        do
        {
            timer += Time.deltaTime / animationTime;
            transform.position = Vector3.Lerp(path[pathIndex].position, path[pathIndex + 1].position, curve.Evaluate(timer));
            yield return wait;
        }
        while (timer < 1f);
        pathIndex++;
        if(pathIndex < path.Count - 1)
        {
            eventCanInteractWithAirCables.Raise();
        }
        else
        {
            eventEndAirCablesMinigame.Raise();
        }
    }
}
