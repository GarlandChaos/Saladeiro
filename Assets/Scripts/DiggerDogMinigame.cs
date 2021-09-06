using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggerDogMinigame : MonoBehaviour
{
    [SerializeField]
    List<GameObject> objectsToThrow = new List<GameObject>();
    [SerializeField]
    Transform dog = null, limitPosition1 = null, limitPosition2 = null, player1 = null, player2 = null;
    Vector3 originalDogPos = Vector3.zero;
    float timer = 0f;
    Vector3 moveLeftDirection = Vector3.zero;
    [SerializeField]
    IntValue points = null;

    private void Awake()
    {
        originalDogPos = dog.position;
        moveLeftDirection = Vector3.Normalize(player1.position - player2.position);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 2f)
        {
            timer = 0f;
            MoveDogAndDig();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            player1.position += moveLeftDirection * 1f;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            player1.position -= moveLeftDirection * 1f;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            player2.position += moveLeftDirection * 1f;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            player2.position -= moveLeftDirection * 1f;
        }
    }

    async void MoveDogAndDig()
    {
        await MoveDogAnimation();
        int rdm = Random.Range(0, objectsToThrow.Count);
        GameObject go = Instantiate(objectsToThrow[rdm], dog.position, objectsToThrow[rdm].transform.rotation);
        ThrowObject to = go.GetComponent<ThrowObject>();
        if(to != null)
        {
            to.originalDogPos = originalDogPos;
            //Debug.Log("to is not null");
        }
    }

    public void AddToPoints()
    {
        points.value += 10;
    }

    IEnumerator MoveDogAnimation()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        Vector3 startPos = dog.position;
        //sorteia qual limite ir
        //calcula a direcao entre a posicao atual e o limite escolhido
        //sorteia o quanto deve andar na direcao calculada
        //se passar do limite escolhido, calcula a magnitude de novo, senão tá pronto
        int rdm = Random.Range(1, 3);
        Vector3 limit = new Vector3();
        if(rdm == 1)
        {
            limit = limitPosition1.position;
        }
        else
        {
            limit = limitPosition2.position;
        }
        float magnitude = Random.Range(1f, Vector3.Distance(startPos, limit));
        Vector3 endPos = startPos + Vector3.Normalize(limit - startPos) * magnitude;
        float timer = 0f;
        float animationTime = 2f;
        do
        {
            timer += Time.deltaTime / animationTime;
            dog.position = Vector3.Lerp(startPos, endPos, curve.Evaluate(timer));
            yield return wait;
        }
        while (timer < 1f);
    }
}
