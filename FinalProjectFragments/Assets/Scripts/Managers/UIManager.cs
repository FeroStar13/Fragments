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

    [SerializeField] private TextMeshProUGUI textDialog;
    [SerializeField] private float typingSpeed;
    [SerializeField] private GameObject choiceCanvas;
    private float activeTypingSpeed;
    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine;

    public Coroutine DisplayLineCoroutine { get => displayLineCoroutine; set => displayLineCoroutine = value; }
    public float ActiveTypingSpeed { get => activeTypingSpeed; set => activeTypingSpeed = value; }
    public float TypingSpeed { get => typingSpeed; }
    public bool CanContinueToNextLine { get => canContinueToNextLine; set => canContinueToNextLine = value; }


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

        //textDialog.text = dialogToDisplay;
        if (DisplayLineCoroutine != null)
        {
            StopCoroutine(DisplayLineCoroutine);
        }
        DisplayLineCoroutine = StartCoroutine("DisplayLine", dialogToDisplay);
        textDialog.color = intendedColor;
    }

    private IEnumerator DisplayLine(string stringToDisplay)
    {
        string str = "<>";
        textDialog.text = "";
        canContinueToNextLine = false;
        foreach (char letter in stringToDisplay)
        {
            if (letter == str[0])
            {
                activeTypingSpeed = 0;
            }
            if (letter == str[1])
            {
                activeTypingSpeed = typingSpeed;
            }

            textDialog.text += letter;


            yield return new WaitForSeconds(ActiveTypingSpeed);


        }
        canContinueToNextLine = true;
    }
    public void ActivateChoiceUi()
    {
        choiceCanvas.SetActive(true);
    }
}
