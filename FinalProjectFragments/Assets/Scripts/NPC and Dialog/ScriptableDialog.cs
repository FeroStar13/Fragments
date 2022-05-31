using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogs")]
public class ScriptableDialog : ScriptableObject
{
    [SerializeField] string _name;
    [SerializeField] string[] dialogs;
    [SerializeField] Color myNameColor;
    [SerializeField] Color myDialogColor;
    [SerializeField] Font myFont;

    public string[] Dialogs { get => dialogs; }
    public Color MyDialogColor { get => myDialogColor; }
    public string Name { get => _name; set => _name = value; }
    public Font MyFont { get => myFont; set => myFont = value; }
    public Color MyNameColor { get => myNameColor; set => myNameColor = value; }
}
