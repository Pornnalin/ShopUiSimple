using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  Manages the shop UI, including item display, sorting,filtering.
/// </summary>
public class ShopUI : MonoBehaviour
{
    [SerializeField] private Transform itemGroup;
    [SerializeField] private Transform togglesGroup;
    [SerializeField] private List<TextMeshProUGUI> itemNameText = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> itemPriceText = new List<TextMeshProUGUI>();
    [SerializeField] private List<Image> itemImages = new List<Image>();
    [SerializeField] private List<CategoryLabel> categories = new List<CategoryLabel>();
    [SerializeField] private List<Toggle> toggles = new List<Toggle>();

    void Start()
    {
        //Find transform by name in hierarchy
        itemGroup = GameObject.Find("ItemGroup").GetComponent<Transform>();
        togglesGroup = GameObject.Find("ToggleGroup").GetComponent<Transform>();

        SpawnUI();
        SetupUIElements();

    }
    /// <summary>
    /// Instantiates item UI prefabs.
    /// </summary>
    private void SpawnUI()
    {
        for (int i = 0; i < SOManager.instance.shopItemSO.shopItemsList.Count; i++)
        {
            GameObject go = Instantiate(Resources.Load("ItemUI", typeof(GameObject)) as GameObject, itemGroup);
            go.transform.SetParent(itemGroup, false);

        }
    }
    /// <summary>
    /// Sets up UI elements by finding and adding components to lists.
    /// </summary>
    private void SetupUIElements()
    {

        //Get all child tranforms from iteamGroup and togglesGroup
        Transform[] transforms = itemGroup.GetComponentsInChildren<Transform>();
        Transform[] transTog = togglesGroup.GetComponentsInChildren<Transform>();

        foreach (Transform t in transforms)
        {
            if (t != itemGroup)
            {

                if (t.name.Contains("Name") && t.TryGetComponent(out TextMeshProUGUI nameText))
                {
                    itemNameText.Add(nameText);
                }
                else if (t.name.Contains("Price") && t.TryGetComponent(out TextMeshProUGUI priceText))
                {
                    itemPriceText.Add(priceText);
                }
                else if (t.name.Contains("Image") && t.TryGetComponent(out Image image))
                {
                    itemImages.Add(image);
                }
            }
        }

        foreach (Transform t in transTog)
        {
            if (t.TryGetComponent(out Toggle toggle))
            {
                toggles.Add(toggle);
            }
        }

        SetShopItemDetails();
    }

    /// <summary>
    /// Updates item details name, price, and image based on data from the shop item scriptable object.
    /// </summary>
    void SetShopItemDetails()
    {
        for (int i = 0; i < SOManager.instance.shopItemSO.shopItemsList.Count; i++)
        {
            var shopItem = SOManager.instance.shopItemSO.shopItemsList[i];
            CategoryLabel category = itemNameText[i].transform.parent.GetComponent<CategoryLabel>();
            categories.Add(category);

            categories[i].categoryName = shopItem.categoryEnum.category.ToString();
            itemNameText[i].text = shopItem.itemName;
            itemPriceText[i].text = shopItem.itemPrice.ToString();
            itemImages[i].sprite = shopItem.itemSprite;
        }
    }
    /// <summary>
    ///  Sorting based on dropdown selection.
    /// </summary>
    /// <param name="dropDown">Dropdown component for sorting.</param>
    public void SortItemsBasedOnDropdown(TMP_Dropdown dropDown)
    {
        switch (dropDown.value)
        {
            case 1:
                //Sort by item name A-Z
                itemNameText.Sort((x, y) => CompareStringText(x.text, y.text));
                UpdateSiblingIndex(itemNameText);
                break;

            case 2:
                //Sort by item name Z-A
                itemNameText.Sort((x, y) => CompareStringText(y.text, x.text));
                UpdateSiblingIndex(itemNameText);
                break;

            case 3:
                // Sort by price low to high
                itemPriceText.Sort((x, y) => CompareNumericText(x.text, y.text));
                UpdateSiblingIndex(itemPriceText);
                break;
            case 4:
                // Sort by price high to low
                itemPriceText.Sort((x, y) => CompareNumericText(y.text, x.text));
                UpdateSiblingIndex(itemPriceText);

                break;

        }
        RefreshUIElementList();
        Debug.Log("SortItem : " + dropDown.options[dropDown.value].text);
    }
    /// <summary>
    /// Compares two numeric text values for sorting.
    /// </summary>
    /// <param name="x">First numeric string</param>
    /// <param name="y">Second numeric string</param>
    /// <returns>Comparion result</returns>
    private int CompareNumericText(string x, string y)
    {
        int numX = int.Parse(x);
        int numY = int.Parse(y);

        return numX.CompareTo(numY);
    }
    /// <summary>
    /// Compares two string for sorting.
    /// </summary>
    /// <param name="x">First string</param>
    /// <param name="y">Second string</param>
    /// <returns>Comparison result</returns>
    private int CompareStringText(string x, string y)
    {
        return string.Compare(x, y);
    }

    // Update the sibling index of UI elements to match the new order
    private void UpdateSiblingIndex(List<TextMeshProUGUI> targetList)
    {
        for (int i = 0; i < targetList.Count; i++)
        {
            targetList[i].transform.parent.SetSiblingIndex(i);
        }
    }

    // Toggle item visibility based on selected category
    public void ToggleOnOffItem(string category)
    {
        switch (category)
        {
            case "Food":
                DisableItem(CategoryEnum.ItemCategory.FOOD);
                break;
            case "Acc":
                DisableItem(CategoryEnum.ItemCategory.ACCESSORIES);
                break;
            case "Potion":
                DisableItem(CategoryEnum.ItemCategory.POTION);
                break;
            case "Other":
                DisableItem(CategoryEnum.ItemCategory.OTHER);
                break;

        }
    }

    // Disables or enables items based on category and toggle state
    void DisableItem(CategoryEnum.ItemCategory itemCategory)
    {
        for (int i = 0; i < categories.Count; i++)
        {
            if (categories[i].categoryName.Contains(itemCategory.ToString()))
            {
                // Disable or enable items
                categories[i].gameObject.SetActive(toggles[((int)itemCategory) - 1].isOn);
                Debug.Log(itemCategory.ToString() + ":" + toggles[((int)itemCategory) - 1].isOn);
            }

        }

    }
    //Refreshes the list of UI
    public void RefreshUIElementList()
    {
        Transform[] transforms = itemGroup.GetComponentsInChildren<Transform>(true);

        categories.Clear();
        itemImages.Clear();
        itemNameText.Clear();
        itemPriceText.Clear();

        foreach (Transform t in transforms)
        {
            if (t.TryGetComponent(out CategoryLabel label))
            {
                categories.Add(label);
            }
        }

        for (int i = 0; i < categories.Count; i++)
        {
            Image newIm = categories[i].transform.GetChild(0).GetComponentInChildren<Image>();
            itemImages.Add(newIm);

            TextMeshProUGUI newName = categories[i].transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
            itemNameText.Add(newName);

            TextMeshProUGUI newPrice = categories[i].transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
            itemPriceText.Add(newPrice);
        }
    }
}
