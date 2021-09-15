using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersController : MonoBehaviour
{
    [SerializeField]
    Transform transformPlayer1 = null;
    [SerializeField]
    Transform transformPlayer2 = null;
    //[SerializeField]
    //Camera cam = null;
    float speed = 3f;
    bool swap =  false;
    [SerializeField]
    GameEvent eventOpenInventory = null, eventCloseInventory = null;
    [SerializeField]
    Animator animator = null;
    int WalkHash = Animator.StringToHash("Walk");
    int WaveHash = Animator.StringToHash("Wave");
    Coroutine followPlayerCoroutine = null;

    private void Awake()
    {
        Interactor player1InteractionComponent = transformPlayer1.GetComponent<Interactor>();
        Interactor player2InteractionComponent = transformPlayer2.GetComponent<Interactor>();
        if (!player1InteractionComponent.enabled)
        {
            player1InteractionComponent.enabled = true;
        }
        if (player2InteractionComponent.enabled)
        {
            player2InteractionComponent.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!swap)
        {
            MovePlayer();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SwapPlayers();
            }
            if (Input.GetKeyDown(KeyCode.X)) //provisório, teste
            {
                animator.SetTrigger(WaveHash);
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            eventOpenInventory.Raise();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            eventCloseInventory.Raise();
        }

    }

    void MovePlayer()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if(move != Vector3.zero)
        {
            transformPlayer1.forward = move;
            animator.SetBool(WalkHash, true);
            transformPlayer1.position += move * speed * Time.deltaTime;
            if(followPlayerCoroutine != null)
            {
                StopCoroutine(followPlayerCoroutine);
            }
            followPlayerCoroutine = StartCoroutine(FollowPlayerCoroutine());
        }
        else
        {
            animator.SetBool(WalkHash, false);
        }
    }

    IEnumerator FollowPlayerCoroutine()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        float timer = 0f;
        float duration = 0.2f;
        Vector3 startPos = transformPlayer2.position;
        Vector3 player1Pos = transformPlayer1.position;
        player1Pos.y = transformPlayer2.position.y;
        Vector3 direction = Vector3.Normalize(player1Pos - startPos);
        Vector3 endPos = player1Pos - direction * 1.5f;
        Vector3 delta = endPos - startPos;
        transformPlayer2.LookAt(player1Pos);

        do
        {
            timer += Time.deltaTime / duration;
            transformPlayer2.position = startPos + delta * curve.Evaluate(timer);
            yield return wait;
        }
        while (timer < 1f);

        followPlayerCoroutine = null;
    }

    public void SwapPlayers()
    {
        swap = true;
        if (followPlayerCoroutine != null)
        {
            StopCoroutine(followPlayerCoroutine);
        }
        transformPlayer2.localRotation = new Quaternion(0, 0, 0, transformPlayer1.rotation.w);
        //await SwapCamera();
        transformPlayer1.GetComponent<Interactor>().enabled = false;
        Transform p = transformPlayer1;
        transformPlayer1 = transformPlayer2;
        transformPlayer2 = p;
        transformPlayer1.GetComponent<Interactor>().enabled = true;
        //cam.transform.SetParent(transformPlayer1);
        swap = false;
    }

    //IEnumerator SwapCamera()
    //{
    //    float timer = 0f;
    //    float animationTime = 2f;
    //    AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    //    Vector3 startPos = cam.transform.position;
    //    Vector3 endPos = transformPlayer2.position + Vector3.back * Vector3.Distance(transformPlayer1.position, startPos) + new Vector3(0, startPos.y, 0);
    //    do
    //    {
    //        timer += Time.deltaTime / animationTime;
    //        cam.transform.position = Vector3.Lerp(startPos, endPos, curve.Evaluate(timer));
    //        yield return new WaitForEndOfFrame();
    //    }
    //    while (timer < 1f);
    //}
}
