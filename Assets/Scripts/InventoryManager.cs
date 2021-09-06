using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance = null;
    List<Item> items = new List<Item>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            items = new List<Item>();
        }
        else
        {
            Destroy(this);
        }
    }

    public void AddToInventory(Item item)
    {
        items.Add(item);
        Debug.Log(item.name + " added to inventory!");
        Debug.Log("Nº of items: " + items.Count);
    }

    public List<Item> GetItems()
    {
        return items;
    }

    //public bool CompareListofItems(List<ItemDisplayTemplate> itemsToCompare)
    //{
    //    foreach(ItemDisplayTemplate i in itemsToCompare)
    //    {
    //        if(items.Contains(i))
    //    }
    //    if(itemsToCompare == items)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
}
