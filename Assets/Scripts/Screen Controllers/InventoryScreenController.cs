using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryScreenController : APanelController
{
    [SerializeField]
    GameObject prefabItemDisplay;
    [SerializeField]
    Transform itemsDisplayArea;
    [SerializeField]
    TMP_Text itemDescriptionText;
    [SerializeField]
    RectTransform selector;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{
      
    //}

    private void OnEnable()
    {
        //List<Item> items = new List<Item>();
        //items = InventoryManager.instance.GetItems();
        selector.gameObject.SetActive(false);
        if (InventoryManager.instance != null)
        {
            if(itemsDisplayArea.childCount > 0)
            {
                for(int i = 0; i < itemsDisplayArea.childCount; i++)
                {
                    Destroy(itemsDisplayArea.GetChild(i).gameObject);
                }
            }
            
            foreach (Item i in InventoryManager.instance.GetItems())
            {
                GameObject item = Instantiate(prefabItemDisplay);
                item.transform.SetParent(itemsDisplayArea, false);
                item.GetComponent<ItemDisplayTemplate>().FillInfo(i);
                item.GetComponent<Button>().onClick.AddListener(delegate { OnSelectItem(i); });
            }
        }
    }

    private void OnSelectItem(Item item)
    {
        itemDescriptionText.text = item._description;
        selector.position = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().position;
        selector.gameObject.SetActive(true);
    }
}
