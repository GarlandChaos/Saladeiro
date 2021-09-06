using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDisplayTemplate : MonoBehaviour
{
    [SerializeField]
    TMP_Text itemName = null;
    public string itemDescription;
    [SerializeField]
    Image itemImage = null;

    public void FillInfo(Item item)
    {
        itemName.text = item._name;
        itemDescription = item._description;
    }
}
