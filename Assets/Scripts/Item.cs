using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IItem
{
    string _name { get; set; }
    string _description { get; set; }
}

public class Item : MonoBehaviour, IItem, IInteractable
{
    [field: SerializeField]
    public string _name { get; set; }
    [field: SerializeField]
    public string _description { get; set; }

    private void Awake()
    {
        if(_name == null)
        {
            _name = "Name not set.";
        }
        if(_description == null)
        {
            _description = "Description not set.";
        }
    }

    public void Interact()
    {
        InventoryManager.instance.AddToInventory(this);
        Debug.Log(name + " added to Inventory.");
        Destroy(gameObject);
    }
}
