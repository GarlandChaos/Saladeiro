using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [SerializeField]
    UIManager prefabUIManager = null;
    [SerializeField]
    InventoryManager prefabInventoryManager = null;
    [SerializeField]
    MessageManager prefabMessageManager = null;
    [SerializeField]
    GameEvent eventUpdateCurrentItemUndergoingExamination = null,
              eventUpdateCurrentItemUndergoingInteraction = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(prefabUIManager);
        Instantiate(prefabInventoryManager);
        Instantiate(prefabMessageManager);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "Main Menu")
        {
            UIManager.instance.RequestScreen("Main Menu", false);
            // UIManager.instance.RequestScreen("Air Cable Minigame Screen", true);
            //UIManager.instance.RequestScreen("Message Dialog", true);
            //UIManager.instance.RequestScreen("HQ Screen", true);
            UIManager.instance.RequestScreen("Digger Dog Minigame Screen", true);
        }
        else
        {
            UIManager.instance.RequestScreen("Main Menu");
        }
    }

    public void OnStartGame()
    {
        Debug.Log("Start game now!!!");
        SceneManager.LoadScene(1);
    }

    public void OnShowItemFloatingButton(Item item)
    {
        UIManager.instance.RequestScreen("Item Floating Button", true);
        eventUpdateCurrentItemUndergoingExamination.Raise(item);

    }

    public void OnHideItemFloatingButton()
    {
        UIManager.instance.RequestScreen("Item Floating Button", false);
    }

    //public void OnItemInteraction(Item item)
    //{
    //    UIManager.instance.RequestScreen("Item Examination", true);
    //    eventUpdateCurrentItemUndergoingInteraction.Raise(item);
    //}

    //public void OnItemCollected(Item item)
    //{
    //    UIManager.instance.RequestScreen("Item Examination", false);
    //    InventoryManager.instance.AddToInventory(item);
    //}

    public void OnOpenInventory()
    {
        if(!UIManager.instance.IsScreenVisible("Inventory Screen"))
        {
            UIManager.instance.RequestScreen("Inventory Screen", true);
        }
    }

    public void OnCloseInventory()
    {
        if(UIManager.instance.IsScreenVisible("Inventory Screen"))
        {
            UIManager.instance.RequestScreen("Inventory Screen", false);
        }
    }

    public void OnEndAirCableMinigame()
    {
        MessageManager.instance.message = "Parabéns, você venceu!";
        UIManager.instance.RequestScreen("Message Dialog", true);
    }

    public void OnCloseMessageDialog()
    {
        UIManager.instance.RequestScreen("Message Dialog", false);
        MessageManager.instance.message = "No message avaiable";
    }
}
