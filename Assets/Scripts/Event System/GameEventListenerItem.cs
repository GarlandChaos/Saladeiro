using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

interface IGameEventListenerItem : IGameEventListener
{
    void OnEventRaised(Item item);
}

[System.Serializable]
public class ItemEvent : UnityEvent<Item> { }

public class GameEventListenerItem : MonoBehaviour, IGameEventListenerItem
{
    public GameEvent gameEvent;
    public ItemEvent response;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        Debug.Log("Cannot use this version");
    }

    public void OnEventRaised(Item item)
    {
        response.Invoke(item);
    }
}
