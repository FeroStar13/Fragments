using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private Image staminaImage;
    [SerializeField] private Image hpImage;

    [SerializeField] private Text npcDialog;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;

        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void UpdateStamina(float staminaDoPlayer)
    {
        staminaImage.fillAmount = staminaDoPlayer / 100.0f;
    }
    public void UpdateHp(float hpDoPlayer)
    {
        hpImage.fillAmount = hpDoPlayer / 100.0f;
    }

    public void DialogOnScrene(string dialogToDisplay, Color intendedColor)
    {
        npcDialog.text = dialogToDisplay;
        npcDialog.color = intendedColor;
       
    }
}