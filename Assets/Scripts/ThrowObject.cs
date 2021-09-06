using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    bool up = true;
    float speedUp = 7f, speedDown = 4.5f;
    public bool valuable = false;
    Camera cam = null;
    Plane[] planes = null;
    Vector3 startPos = Vector3.zero;
    public Vector3 originalDogPos = Vector3.zero;
    [SerializeField]
    GameEvent eventAddToDiggerDogPoints = null;

    private void Awake()
    {
        startPos = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {    
        cam = GameObject.FindGameObjectWithTag("Digger Dog Minigame Camera").GetComponent<Camera>();
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider>().bounds))
        {
            up = false;
            Vector3 originalFallPos = new Vector3(originalDogPos.x, transform.position.y, originalDogPos.z);
            Vector3 camReference = new Vector3(cam.transform.position.x, transform.position.y, cam.transform.position.z);
            Vector3 normal = Vector3.Normalize(camReference - originalFallPos);
            Vector3 fallPos = new Vector3(startPos.x, transform.position.y, startPos.z);
            transform.position = fallPos + normal * 6.5f;
        }

        if (up)
        {
            transform.position += Vector3.up * speedUp * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.down * speedDown * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(name + "entered trigger");
        if (!up)
        {
            if (other.tag == "Grass")
            {
                Destroy(gameObject);
            }
            else if(other.tag == "PlayerBasket")
            {
                if (valuable)
                {
                    eventAddToDiggerDogPoints.Raise();
                }
                Destroy(gameObject);
            }
        }   
    }
}
