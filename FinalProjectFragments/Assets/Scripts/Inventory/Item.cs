

using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    [SerializeField] string itemName;
    [SerializeField] Sprite Icon;
    [SerializeField] int ammount;

    public Sprite Icon1 { get => Icon; set => Icon = value; }
}
