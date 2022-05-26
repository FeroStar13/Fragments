using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Bars")]
    [SerializeField] private Image staminaImage;
    [SerializeField] private Image hpImage;
    [SerializeField] private Image chargeImage;

    [Header("Dialog")]
    [SerializeField] private Text npcDialog;

    [Header("Inventory")]
    [SerializeField] GameObject Inventory;

    [SerializeField] GameObject InventoryBackground;
    [SerializeField] GameObject CraftingBackground;

    [SerializeField] bool _inventoryIsOpen = false;

    [Header("SpecialWeapons")]

    [SerializeField] GameObject ExplosiveBoxInUI;
    [SerializeField] GameObject SuperCannonInUI;

    [Header("BossHealthBar")]

    [SerializeField] Image _bossHpBarImage;

    [Header("Reference")]
    [SerializeField] PlayerCharacter player;

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
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void Update()
    {
        OpenInventory();
    }
    public void UpdateStamina(float staminaDoPlayer)
    {
        staminaImage.fillAmount = staminaDoPlayer / 100.0f;
    }
    public void UpdateHp(float hpDoPlayer)
    {
        hpImage.fillAmount = hpDoPlayer / 100.0f;
    }
    public void UpdateCharge(int chargeDoPlayer)
    {
        chargeImage.fillAmount = chargeDoPlayer / 100.0f;
    }

    public void DialogOnScrene(string dialogToDisplay, Color intendedColor)
    {
        npcDialog.text = dialogToDisplay;
        npcDialog.color = intendedColor;

    }

    public void InventoryButton()
    {
        InventoryBackground.SetActive(true);
        CraftingBackground.SetActive(false);
    }
    public void CraftingButton()
    {
        InventoryBackground.SetActive(false);
        CraftingBackground.SetActive(true);
    }

    public void isExplosiveBox()
    {
        ExplosiveBoxInUI.SetActive(true);
        SuperCannonInUI.SetActive(false);
    }
    public void isSuperCannon()
    {
        ExplosiveBoxInUI.SetActive(false);
        SuperCannonInUI.SetActive(true);
    }

    void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I) && _inventoryIsOpen == false)
        {
            _inventoryIsOpen = true;
            player.CanFire = false;
            Inventory.SetActive(_inventoryIsOpen);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            return;

        }
        if(Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape) && _inventoryIsOpen == true)
        {
            _inventoryIsOpen = false;
            player.CanFire = true;
            Inventory.SetActive(_inventoryIsOpen);
            Cursor.lockState = CursorLockMode.Confined  ;
            Cursor.visible = false;
            return;
        }
    
    }
    public void UpdateBossHp(float hpDoBoss)
    {
        _bossHpBarImage.fillAmount = hpDoBoss / 1000;
    }
}