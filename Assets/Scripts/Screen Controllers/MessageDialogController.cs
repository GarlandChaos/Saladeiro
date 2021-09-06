using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MessageDialogController : ADialogController
{
    [SerializeField]
    TMP_Text message;
    [SerializeField]
    Button buttonConfirm;
    [SerializeField]
    GameEvent eventCloseMessageDialog;

    private void OnEnable()
    {
        if(MessageManager.instance != null)
        {
            message.text = MessageManager.instance.message;
        }
        buttonConfirm.Select();
    }

    public void CloseDialog()
    {
        eventCloseMessageDialog.Raise();
    }
}
