using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExaminationController : ADialogController
{
    Item item;
    [SerializeField]
    GameEvent eventItemCollected;

    public void ColectItem()
    {
        Debug.Log("Collected item.");
        eventItemCollected.Raise(item);
    }

    public void UpdateItem(Item itm)
    {
        item = itm;
        Debug.Log(item.name + " updated in examination controller.");
    }
}
