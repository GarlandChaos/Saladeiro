using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CattleMinigameManager : MonoBehaviour
{
    public static CattleMinigameManager instance = null;
    List<CowController> cowList = new List<CowController>();
    [SerializeField]
    List<GameObject> objectList = new List<GameObject>();
    CowController cowCurrent = null;
    [SerializeField]
    GameObject doorToOpen = null;
    [SerializeField]
    Camera mainCamera = null;
    [SerializeField]
    Camera camera1 = null;
    [SerializeField]
    Camera cameraDoor = null;
    [SerializeField]
    Camera camera2 = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterCow(CowController cow)
    {
        cowList.Add(cow);
    }

    public void UnregisterCow(CowController cow)
    {
        SpawnObject(cow.transform.position);
        cowList.Remove(cow);
        if(cowList.Count == 0)
        {
            //trigger win event here
            StartCoroutine(ChangeCameraCoroutine(mainCamera, camera1, cameraDoor, true));
            StartCoroutine(OpenDoorCoroutine());
        }
    }

    public void SetCurrentCow(CowController cow)
    {
        foreach(CowController c in cowList)
        {
            if(c != cow)
            {
                c.Stop();
            }
        }
        cowCurrent = cow;
    }

    void SpawnObject(Vector3 position)
    {
        if(objectList.Count != 0)
        {
            GameObject obj = objectList[objectList.Count - 1];
            objectList.Remove(obj);
            Instantiate(obj, position, obj.transform.rotation);
        }
    }

    public void ChangeToCamera1()
    {
        if(mainCamera.transform.position != camera1.transform.position)
        {
            StartCoroutine(ChangeCameraCoroutine(mainCamera, camera2, camera1));
        }
    }

    public void ChangeToCamera2()
    {
        if(mainCamera.transform.position != camera2.transform.position)
        {
            StartCoroutine(ChangeCameraCoroutine(mainCamera, camera1, camera2));
        }
    }

    IEnumerator OpenDoorCoroutine()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        float timer = 0f;
        float animationTime = 2.5f;
        Quaternion startRot = doorToOpen.transform.rotation;
        Quaternion endRot = startRot;
        endRot.SetFromToRotation(doorToOpen.transform.right, -doorToOpen.transform.forward);

        do
        {
            timer += Time.deltaTime / animationTime;
            doorToOpen.transform.rotation = Quaternion.Slerp(startRot, endRot, curve.Evaluate(timer));
            yield return wait; 
        }
        while (timer < 1f);

    }

    IEnumerator ChangeCameraCoroutine(Camera camToChange, Camera cam1, Camera cam2, bool changeBackAtEnd = false)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        float timer = 0f;
        float animationTime = 2.5f;
        Vector3 startPos = cam1.transform.position;
        Vector3 endPos = cam2.transform.position;
        Vector3 deltaPos = endPos - startPos;
        Quaternion startRot = cam1.transform.rotation;
        Quaternion endRot = cam2.transform.rotation;

        do
        {
            timer += Time.deltaTime / animationTime;
            camToChange.transform.position = startPos + deltaPos * curve.Evaluate(timer);
            camToChange.transform.rotation = Quaternion.Slerp(startRot, endRot, curve.Evaluate(timer));
            yield return wait;
        }
        while (timer < 1f);

        if (changeBackAtEnd)
        {
            timer = 0f;
            float pauseTime = 2f;
            do
            {
                timer += Time.deltaTime / pauseTime;
                yield return wait;
            }

            while (timer < 1f);

            timer = 0f;
            startPos = cam2.transform.position;
            endPos = cam1.transform.position;
            deltaPos = endPos - startPos;
            startRot = cam2.transform.rotation;
            endRot = cam1.transform.rotation;

            do
            {
                timer += Time.deltaTime / animationTime;
                camToChange.transform.position = startPos + deltaPos * curve.Evaluate(timer);
                camToChange.transform.rotation = Quaternion.Slerp(startRot, endRot, curve.Evaluate(timer));
                yield return wait;
            }
            while (timer < 1f);
        }
    }
}
