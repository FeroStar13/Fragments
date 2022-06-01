using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialog : MonoBehaviour, IInteractable
{
    [SerializeField] ScriptableDialog myDialogTree;
    [SerializeField] GameObject dialogBackGround;

    bool _destroyExclamation = true;

    int positionInDialog;

    public void Interact(PlayerCharacter playerThatInteract)
    {
        Conversation();
        if(_destroyExclamation == true)
        {
            Destroy(transform.GetChild(0).gameObject);
            _destroyExclamation = false;

        }
    }

    void Conversation()
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
