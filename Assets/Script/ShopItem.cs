using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains a list of shop items.
/// </summary>
[CreateAssetMenu(fileName = "ShopItemSO", menuName = "ScriptableObjects/ShopItemSO", order = 1)]
public class ShopItem : ScriptableObject
{
    public List<ItemInfo> shopItemsList;
}

// Information about a shop item.
[System.Serializable]
public class ItemInfo
{
    public string itemName;
    public CategoryEnum categoryEnum;   
    public int itemPrice;   
    public Sprite itemSprite;
}

// Item categories enumeration.
[System.Serializable]
public class CategoryEnum
{
    public enum ItemCategory
    {
        NONE,
        FOOD,
        ACCESSORIES,
        POTION,
        OTHER
    }
    public ItemCategory category;
}
