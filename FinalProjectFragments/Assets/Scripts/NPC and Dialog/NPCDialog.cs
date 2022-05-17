using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialog : MonoBehaviour, IInteractable
{
    [SerializeField] List<ScriptableDialog> dialogList = new List<ScriptableDialog>();
    int position;
    int char1PosInDialog;
    string canvasActive;
    GameObject activeUI;
    UIManager UIManager;

    private void Start()
    {
        UIManager = UIManager.instance;
        position = 1;
        char1PosInDialog = 0;


      

    }
 

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.Space))
        {
            if (UIManager.CanContinueToNextLine == false)
            {
                UIManager.ActiveTypingSpeed = 0.0001f;
                Debug.Log("NotNUll");
            }
            else
            {
                UIManager.ActiveTypingSpeed = UIManager.TypingSpeed;
                Debug.Log("null");
                DialogueRun();
            }

        }

    }

    void DialogueRun()
    {
        foreach (var item in dialogList)
        {
            if (position == item.PositionInScene1) // verificação da pessoa que esta a falar
            {
                if (canvasActive != item.PersonUI.name)
                {
                    Destroy(activeUI);
                    activeUI = Instantiate(item.PersonUI);
                    canvasActive = item.PersonUI.name;
                }
                UIManager.instance.DialogOnScrene(item.Dialogs[char1PosInDialog], item.MyColor);
                char1PosInDialog++;
                if (char1PosInDialog >= item.Dialogs.Length)
                {
                    position++;
                    char1PosInDialog = 0;
                }
                return;
            }
        }
        UIManager.ActivateChoiceUi();
    }

    public void Interact(PlayerCharacter playerThatInteract)
    {
        Debug.Log("interect");
        DialogueRun();
          
        
    }
}
