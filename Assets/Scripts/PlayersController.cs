using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersController : MonoBehaviour
{
    [SerializeField]
    Rigidbody rigidbodyPlayer1 = null;
    [SerializeField]
    Rigidbody rigidbodyPlayer2 = null;
    //[SerializeField]
    //Transform transformPlayer1 = null;
    //[SerializeField]
    //Transform transformPlayer2 = null;
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
        rigidbodyPlayer1.interpolation = RigidbodyInterpolation.Interpolate;
        rigidbodyPlayer2.interpolation = RigidbodyInterpolation.Interpolate;
        rigidbodyPlayer2.isKinematic = true;
        Interactor player1InteractionComponent = rigidbodyPlayer1.GetComponent<Interactor>();
        Interactor player2InteractionComponent = rigidbodyPlayer2.GetComponent<Interactor>();
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
            //MovePlayer();
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

    private void FixedUpdate()
    {
        if (!swap)
        {
            MovePlayer();
        }
    }

    void MovePlayer()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if(move != Vector3.zero)
        {
            rigidbodyPlayer1.transform.forward = move;

            animator.SetBool(WalkHash, true);

            Vector3 newPosition = rigidbodyPlayer1.position + move * speed * Time.fixedDeltaTime;

            Vector3 newPositionOffset = newPosition;
            newPositionOffset.y += 0.5f;

            Vector3 playerPositionOffset = rigidbodyPlayer1.position;
            playerPositionOffset.y += 0.5f;

            Vector3 direction = Vector3.Normalize(newPositionOffset - playerPositionOffset);

#if UNITY_EDITOR
            Debug.DrawRay(playerPositionOffset, direction, Color.red);
#endif
            RaycastHit hit;

            if (!Physics.Raycast(playerPositionOffset, direction, out hit, 0.5f))
            {
                rigidbodyPlayer1.MovePosition(newPosition);
            }

            if (followPlayerCoroutine == null)
            {
                followPlayerCoroutine = StartCoroutine(FollowPlayerCoroutine());
            }
        }
        else
        {
            animator.SetBool(WalkHash, false);
        }
    }

    IEnumerator FollowPlayerCoroutine()
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        float timer = 0f;
        float duration = 0.2f;
        Vector3 startPos = rigidbodyPlayer2.position;
        Vector3 player1Pos = rigidbodyPlayer1.position;
        player1Pos.y = rigidbodyPlayer2.position.y;
        Vector3 direction = Vector3.Normalize(player1Pos - startPos);
        Vector3 endPos = player1Pos - direction * 1.5f;
        Vector3 delta = endPos - startPos;

        Quaternion startRot = rigidbodyPlayer2.rotation;
        Quaternion endRot = Quaternion.LookRotation(direction);

        do
        {
            timer += Time.fixedDeltaTime / duration;
            rigidbodyPlayer2.MovePosition(startPos + delta * curve.Evaluate(timer));
            rigidbodyPlayer2.MoveRotation(Quaternion.Slerp(startRot, endRot, curve.Evaluate(timer)));

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
            followPlayerCoroutine = null;
        }
        rigidbodyPlayer1.GetComponent<Interactor>().enabled = false;
        Rigidbody p = rigidbodyPlayer1;
        rigidbodyPlayer1 = rigidbodyPlayer2;
        rigidbodyPlayer2 = p;
        rigidbodyPlayer1.GetComponent<Interactor>().enabled = true;
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
