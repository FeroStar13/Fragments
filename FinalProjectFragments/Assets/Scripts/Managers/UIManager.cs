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
    [SerializeField] private Text npcName;

    [Header("Inventory")]
    [SerializeField] GameObject Inventory;
   [SerializeField] bool _canOpenInventory;
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

    [Header("Options")]
    [SerializeField] GameObject ControlUI;
    [SerializeField] GameObject AudioUI;
    [SerializeField] GameObject VideoUI;
    [SerializeField] GameObject PauseUI;
    bool _gameEsc;

    [Header("DeathMensage")]
    [SerializeField] GameObject DeathUI;

    [Header("Mouse")]
    [SerializeField] bool _mouseActive = false;

    [Header("Respawn")]
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] Vector3 _playerSpawn;

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
        _mouseActive = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = _mouseActive;
    }

    private void Update()
    {
        
        if (_canOpenInventory == true)
        {
            OpenInventory();

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_gameEsc)
            {
               ResumeGame();
               
                _gameEsc = false;
            }
            else
            {
                Pause();
             
                _gameEsc = true;
            }
        }

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

    public void DialogOnScrene(string dialogToDisplay, Color intendedColor, Font intendedFont)
    {
        npcDialog.text = dialogToDisplay;
        npcDialog.color = intendedColor;
       
        npcDialog.font = intendedFont;

    }
    public void nameDialogOnScrene(string name, Font intendedFont, Color intendedColor)
    {
        npcName.text = name;
        npcName.font = intendedFont;
        npcName.color = intendedColor;
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
        if (Input.GetKeyDown(KeyCode.I) && _inventoryIsOpen == false )
        {
            _inventoryIsOpen = true;
            player.CanFire = false;
            _gameEsc = false;
          
            Inventory.SetActive(_inventoryIsOpen);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            return;

        }
        if(Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape) && _inventoryIsOpen == true)
        {
            _inventoryIsOpen = false;
            player.CanFire = true;
            _gameEsc = true;
         
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

    public void ChangeScene(string sceneName)
    {
        _mouseActive = true;
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()

    {
        Application.Quit();
    }
    public void ControlOpen()
    {
        ControlUI.SetActive(true);
        AudioUI.SetActive(false);
        VideoUI.SetActive(false);
    }
    public void AudioOpen()
    {
        ControlUI.SetActive(false);
        AudioUI.SetActive(true);
        VideoUI.SetActive(false);
    }
    public void VideoOpen()
    {
        ControlUI.SetActive(false);
        AudioUI.SetActive(false);
        VideoUI.SetActive(true);
    }
    
    private void Pause()
    {
        _canOpenInventory = false;
        Time.timeScale = 0;
        _mouseActive = true;
        PauseUI.SetActive(true);
    }
    public void ResumeGame()
    {
        _mouseActive = false;
        _canOpenInventory = true;
        Time.timeScale = 1;       
       PauseUI.gameObject.SetActive(false);

    }

    public void Death()
    {
        DeathUI.SetActive(true);

    }

    public void Respawn()
    {
        DeathUI.SetActive(false);
        player.transform.position = _playerSpawn;
        player.gameObject.SetActive(true);
    }
}