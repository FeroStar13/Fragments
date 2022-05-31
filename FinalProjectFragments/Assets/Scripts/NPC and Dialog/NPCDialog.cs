using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialog : MonoBehaviour, IInteractable
{
    [SerializeField] ScriptableDialog myDialogTree;
    [SerializeField] GameObject dialogBackGround;

    int positionInDialog;

    public void Interact(PlayerCharacter playerThatInteract)
    {
        Conversation();
    }

    void Conversation()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            positionInDialog++;

            dialogBackGround.gameObject.SetActive(true);

            if (positionInDialog >= myDialogTree.Dialogs.Length)
            {
                dialogBackGround.gameObject.SetActive(false);
                positionInDialog = 0;
            }
            UIManager.instance.DialogOnScrene(myDialogTree.Dialogs[positionInDialog], myDialogTree.MyDialogColor,  myDialogTree.MyFont);
            UIManager.instance.nameDialogOnScrene(myDialogTree.Name, myDialogTree.MyFont, myDialogTree.MyNameColor);
        }
    }
}
