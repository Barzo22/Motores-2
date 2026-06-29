using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Custom/Create Item", order = 0)]
public class ShopItem : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public Sprite skinSprite;
    public int itemCost;
    public Color particleColor = Color.white;
}
