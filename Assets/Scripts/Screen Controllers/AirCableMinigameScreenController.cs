using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirCableMinigameScreenController : APanelController
{
    [SerializeField]
    Image bar1Img, bar2Img, bar1LimitImg, bar2LimitImg, correctImg1, correctImg2, incorrectImg1, incorrectImg2, button1BackImg, button2BackImg;
    Vector3 upLimitPos, downLimitPos, originalButton1Pos, originalButton2Pos;
    float limit1Percentage, limit2Percentage;
    public float amountToFill, amountToDecrease;
    [SerializeField]
    RectTransform rectTransformButton1, rectTransformButton2;
    [SerializeField]
    GameEvent eventActivateBasketCamera, eventDeactivateBasketCamera, eventMoveBasket;
    Coroutine button1Coroutine, button2Coroutine;
    float correctMargin;
    bool canInteract;

    private void Awake()
    {
        upLimitPos = new Vector3(0, bar1Img.rectTransform.sizeDelta.y / 2 - 10, -2);
        downLimitPos = new Vector3(0, -bar1Img.rectTransform.sizeDelta.y / 2 -10, -2);
        bar1Img.fillAmount = 0;
        bar2Img.fillAmount = 0;
        amountToFill = 1;
        amountToDecrease = 0.1f;
        bar1LimitImg.rectTransform.localPosition = new Vector3(0, 0, -2);
        bar2LimitImg.rectTransform.localPosition = new Vector3(0, 0, -2);
        limit1Percentage = Random.Range(0.3f, 1f);
        bar1LimitImg.rectTransform.localPosition = Vector3.Lerp(downLimitPos, upLimitPos, limit1Percentage);
        limit2Percentage = Random.Range(0.3f, 1f);
        bar2LimitImg.rectTransform.localPosition = Vector3.Lerp(downLimitPos, upLimitPos, limit2Percentage);
        originalButton1Pos = rectTransformButton1.localPosition;
        originalButton2Pos = rectTransformButton2.localPosition;
        correctMargin = 0.02f;
        canInteract = true;
    }

    private void OnEnable()
    {
        eventActivateBasketCamera.Raise();
        EnableButton1();
        EnableButton2();
    }

    private void OnDisable()
    {
        eventDeactivateBasketCamera.Raise();
        DisableButton1();
        DisableButton2();
    }

    public void DisableButton1()
    {
        StopCoroutine(button1Coroutine);
        button1Coroutine = null;
        rectTransformButton1.localPosition = originalButton1Pos;
        rectTransformButton1.gameObject.SetActive(false);
        button1BackImg.gameObject.SetActive(false);
    }

    public void DisableButton2()
    {
        StopCoroutine(button2Coroutine);
        button2Coroutine = null;
        rectTransformButton2.localPosition = originalButton2Pos;
        rectTransformButton2.gameObject.SetActive(false);
        button2BackImg.gameObject.SetActive(false);
    }

    public void EnableButton1()
    {
        rectTransformButton1.gameObject.SetActive(true);
        button1BackImg.gameObject.SetActive(true);
        rectTransformButton1.localPosition = originalButton1Pos;
        if(button1Coroutine == null)
        {
            button1Coroutine = StartCoroutine(ButtonMoveAnimation(rectTransformButton1, originalButton1Pos, button1Coroutine));
        }
    }

    public void EnableButton2()
    {
        rectTransformButton2.gameObject.SetActive(true);
        button2BackImg.gameObject.SetActive(true);
        rectTransformButton2.localPosition = originalButton2Pos;
        if(button2Coroutine == null)
        {
            button2Coroutine = StartCoroutine(ButtonMoveAnimation(rectTransformButton2, originalButton2Pos, button2Coroutine));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract)
        {
            if (bar1Img.fillAmount <= limit1Percentage + correctMargin &&
            bar1Img.fillAmount >= limit1Percentage - correctMargin)
            {
                if (incorrectImg1.gameObject.activeSelf)
                {
                    incorrectImg1.gameObject.SetActive(false);
                }
                if (rectTransformButton1.gameObject.activeSelf)
                {
                    DisableButton1();
                }
                correctImg1.gameObject.SetActive(true);
            }
            else if (bar1Img.fillAmount > limit1Percentage + correctMargin && canInteract)
            {
                if (correctImg1.gameObject.activeSelf)
                {
                    correctImg1.gameObject.SetActive(false);
                }
                if (rectTransformButton1.gameObject.activeSelf)
                {
                    DisableButton1();
                }
                incorrectImg1.gameObject.SetActive(true);
            }
            else if (bar1Img.fillAmount < limit1Percentage - correctMargin)
            {
                if (correctImg1.gameObject.activeSelf)
                {
                    correctImg1.gameObject.SetActive(false);
                }
                if (!rectTransformButton1.gameObject.activeSelf)
                {
                    EnableButton1();
                }
            }

            if (bar2Img.fillAmount >= limit2Percentage &&
                bar2Img.fillAmount < limit2Percentage + correctMargin)
            {
                if (incorrectImg2.gameObject.activeSelf)
                {
                    incorrectImg2.gameObject.SetActive(false);
                }
                if (rectTransformButton2.gameObject.activeSelf)
                {
                    DisableButton2();
                }
                correctImg2.gameObject.SetActive(true);
            }
            else if (bar2Img.fillAmount > limit2Percentage + correctMargin && canInteract)
            {
                if (correctImg2.gameObject.activeSelf)
                {
                    correctImg2.gameObject.SetActive(false);
                }
                if (rectTransformButton2.gameObject.activeSelf)
                {
                    DisableButton2();
                }
                incorrectImg2.gameObject.SetActive(true);
            }
            else if (bar2Img.fillAmount < limit2Percentage - correctMargin)
            {
                if (correctImg2.gameObject.activeSelf)
                {
                    correctImg2.gameObject.SetActive(false);
                }
                if (!rectTransformButton2.gameObject.activeSelf)
                {
                    EnableButton2();
                }
            }
        }

        if (bar1Img.fillAmount <= limit1Percentage + correctMargin &&
            bar1Img.fillAmount >= limit1Percentage - correctMargin &&
            bar2Img.fillAmount <= limit2Percentage + correctMargin &&
            bar2Img.fillAmount >= limit2Percentage - correctMargin &&
            canInteract)
        {
            canInteract = false;
            eventMoveBasket.Raise();
        }
        else if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                bar1Img.fillAmount += amountToFill * Time.deltaTime;
            }
            else
            {
                if (bar1Img.fillAmount > 0f)
                {
                    bar1Img.fillAmount -= amountToDecrease * Time.deltaTime;
                }
                else
                {
                    bar1Img.fillAmount = 0f;
                }
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                bar2Img.fillAmount += amountToFill * Time.deltaTime;
            }
            else
            {
                if (bar2Img.fillAmount > 0f)
                {
                    bar2Img.fillAmount -= amountToDecrease * Time.deltaTime;
                }
                else
                {
                    bar2Img.fillAmount = 0f;
                }
            }
        }  
    }

    public void ActivateInteraction()
    {
        bar1Img.fillAmount = 0;
        bar2Img.fillAmount = 0;
        limit1Percentage = Random.Range(0.3f, 1f);
        bar1LimitImg.rectTransform.localPosition = Vector3.Lerp(downLimitPos, upLimitPos, limit1Percentage);
        limit2Percentage = Random.Range(0.3f, 1f);
        bar2LimitImg.rectTransform.localPosition = Vector3.Lerp(downLimitPos, upLimitPos, limit2Percentage);
        canInteract = true;
    }

    IEnumerator ButtonMoveAnimation(RectTransform rt, Vector3 originalPos, Coroutine crt)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        Vector3 startPos = originalPos;
        Vector3 endPos = originalPos - new Vector3(0f, 5f, 0f);
        float timer = 0f;
        float animationTime = 0.5f;
        do
        {
            timer += Time.deltaTime / animationTime;
            rt.localPosition = Vector3.Lerp(startPos, endPos, curve.Evaluate(timer));
            yield return wait;
        }
        while (timer < 1f);

        timer = 0f;
        startPos = originalPos - new Vector3(0f, 5f, 0f);
        endPos = originalPos;
        do
        {
            timer += Time.deltaTime / animationTime;
            rt.localPosition = Vector3.Lerp(startPos, endPos, curve.Evaluate(timer));
            yield return wait;
        }
        while (timer < 1f);

        crt = null;
        crt = StartCoroutine(ButtonMoveAnimation(rt, originalPos, crt));
    }
}
