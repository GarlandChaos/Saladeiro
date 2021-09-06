using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersController : MonoBehaviour
{
    [SerializeField]
    Transform transformPlayer1 = null;
    [SerializeField]
    Transform transformPlayer2 = null;
    [SerializeField]
    Camera cam = null;
    float speed = 3f;
    bool swap =  false;
    [SerializeField]
    GameEvent eventOpenInventory = null, eventCloseInventory = null;
    [SerializeField]
    Animator animator = null;
    int WalkHash = Animator.StringToHash("Walk");
    int WaveHash = Animator.StringToHash("Wave");

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
            MovePlayer1();
            FollowPlayer1();
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

    void MovePlayer1()
    {
        //não funciona em pc remoto...
        //float horizontal = Input.GetAxis("Mouse X");
        //Debug.Log(horizontal);
        //transformPlayer1.Rotate(horizontal * 30f * Vector3.up, Space.World);

        if (Input.GetKey(KeyCode.Q))
        {
            transformPlayer1.Rotate(Vector3.down * 30f * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transformPlayer1.Rotate(Vector3.up * 30f * Time.deltaTime);
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if(move != Vector3.zero)
        {
            transformPlayer1.forward = move;
            animator.SetBool(WalkHash, true);
            transformPlayer1.position += move * speed * Time.deltaTime;
        }
        else
        {
            animator.SetBool(WalkHash, false);
        }

        //if (Input.GetKey(KeyCode.W))
        //{
        //    animator.SetBool(WalkHash, true);
        //    transformPlayer1.position += transformPlayer1.forward * speed * Time.deltaTime;
        //}
        //else if (Input.GetKey(KeyCode.S))
        //{
        //    animator.SetBool(WalkHash, true);
        //    Vector3 direction = transformPlayer1.position - transformPlayer1.forward;
        //    //transformPlayer1.rotation = Quaternion.LookRotation(direction, Vector3.up);
        //    transform.forward = direction;
        //    transformPlayer1.position -= transformPlayer1.forward * speed * Time.deltaTime;
        //}
        //else if (Input.GetKey(KeyCode.A))
        //{
        //    animator.SetBool(WalkHash, true);
        //    transformPlayer1.position -= transformPlayer1.right * speed * Time.deltaTime;
        //}
        //else if (Input.GetKey(KeyCode.D))
        //{
        //    animator.SetBool(WalkHash, true);
        //    transformPlayer1.position += transformPlayer1.right * speed * Time.deltaTime;
        //}

        //if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        //{
        //    animator.SetBool(WalkHash, false);
        //}
    }

    void FollowPlayer1()
    {
        transformPlayer2.LookAt(transformPlayer1);

        if (Vector3.Distance(transformPlayer1.position, transformPlayer2.position) > 4f)
        {
            transformPlayer2.position += Vector3.Normalize(transformPlayer1.position - transformPlayer2.position) * speed * Time.deltaTime;
        }
    }

    public async void SwapPlayers()
    {
        swap = true;
        transformPlayer2.localRotation = new Quaternion(0, 0, 0, transformPlayer1.rotation.w);
        await SwapCamera();
        //Destroy(transformPlayer1.GetComponent<PlayerInteraction>());
        transformPlayer1.GetComponent<Interactor>().enabled = false;
        Transform p = transformPlayer1;
        transformPlayer1 = transformPlayer2;
        transformPlayer2 = p;
        transformPlayer1.GetComponent<Interactor>().enabled = true;
        cam.transform.SetParent(transformPlayer1);
        swap = false;
    }

    IEnumerator SwapCamera()
    {
        float timer = 0f;
        float animationTime = 2f;
        AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        Vector3 startPos = cam.transform.position;
        Vector3 endPos = transformPlayer2.position + Vector3.back * Vector3.Distance(transformPlayer1.position, startPos) + new Vector3(0, startPos.y, 0);
        do
        {
            timer += Time.deltaTime / animationTime;
            cam.transform.position = Vector3.Lerp(startPos, endPos, curve.Evaluate(timer));
            yield return new WaitForEndOfFrame();
        }
        while (timer < 1f);
    }
}
