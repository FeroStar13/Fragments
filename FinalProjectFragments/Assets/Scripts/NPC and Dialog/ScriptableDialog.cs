using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogs")]
public class ScriptableDialog : ScriptableObject
{
    [SerializeField] string[] dialogs;
    [SerializeField] Color myColor;
    [SerializeField] int PositionInScene;
    [SerializeField] GameObject personUI;


    public string[] Dialogs { get => dialogs; }    public Color MyColor { get => myColor; }
    public int PositionInScene1 { get => PositionInScene; set => PositionInScene = value; }
    public GameObject PersonUI { get => personUI; set => personUI = value; }
}
