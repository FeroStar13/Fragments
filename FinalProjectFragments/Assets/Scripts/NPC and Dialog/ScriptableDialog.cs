using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogs")]
public class ScriptableDialog : ScriptableObject
{

    [SerializeField] string[] dialogs;
    [SerializeField] Color myColor;


    public string[] Dialogs { get => dialogs; }
    public Color MyColor { get => myColor; }
    
}
