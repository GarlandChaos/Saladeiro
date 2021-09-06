using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiggerDogMinigameScreenController : APanelController
{
    [SerializeField]
    TMP_Text pointsText = null;
    [SerializeField]
    IntValue points = null;

    private void OnEnable()
    {
        points.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePoints(); //rever para não ser a cada frame...
    }

    public void UpdatePoints()
    {
        pointsText.text = "Pontos: " + points.value.ToString();
    }
}
