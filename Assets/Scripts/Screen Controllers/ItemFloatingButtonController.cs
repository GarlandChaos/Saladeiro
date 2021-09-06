using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemFloatingButtonController : APanelController
{
    [SerializeField]
    Image floatingButtonImg;
    Item currentItem;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ItemFloatingButtonControllerLoaded");
    }

    // Update is called once per frame
    void Update()
    {
        if (currentItem != null)
        {
            UpdateFloatingButtonPosition();
        }
    }

    private void OnDisable()
    {
        currentItem = null;
    }

    public void UpdateCurrentItem(Item item)
    {
        currentItem = item;
        Debug.Log(item.name + " updated in itemFloating controller.");
        UpdateFloatingButtonPosition();
    }

    public void UpdateFloatingButtonPosition()
    {
        floatingButtonImg.transform.position = currentItem.transform.position;
        //// Final position of marker above GO in world space
        //Vector3 offsetPos = new Vector3(currentItem.transform.position.x, currentItem.transform.position.y, currentItem.transform.position.z);
        //// Calculate *screen* position (note, not a canvas/recttransform position)
        //Vector2 canvasPos;
        //Vector2 screenPoint = UIManager.instance._UICamera.WorldToScreenPoint(offsetPos);
        //// Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(UIManager.instance._MainCanvas.GetComponent<RectTransform>(), screenPoint, UIManager.instance._UICamera, out canvasPos);
        ////floatingButtonImg.SetActive(true);
        //floatingButtonImg.transform.localPosition = canvasPos /*+ IndicatorImageOffset*/;
        ////IndicatorImage.GetComponent<IndicatorImageController>().CurrentObjectSelected = hit.transform;

        //Vector2 canvasPos;
        //Vector2 screenPos = Camera.main.WorldToScreenPoint(currentItem.transform.position);
        //Debug.Log("screenPos: " + screenPos);
        ////RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.GetComponentInParent<RectTransform>(), screenPos, Camera.main, out canvasPos);
        //floatingButtonImg.transform.localPosition = canvasPos/* + new Vector2(0, 894.9209f)*/;


    }
}
