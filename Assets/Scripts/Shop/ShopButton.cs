using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopButton : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    [SerializeField] TMP_Text itemNameText;
    [SerializeField] TMP_Text itemCostText;
    [SerializeField] Button buyButton;
    [SerializeField] TMP_Text buyButtonText;

    [SerializeField] Color affordableColor = new Color(0.2f, 0.8f, 0.2f);
    [SerializeField] Color unaffordableColor = new Color(0.8f, 0.2f, 0.2f);
    [SerializeField] Color equippedColor = new Color(0.8f, 0.8f, 0.2f);

    ShopItem item;

    void Awake()
    {
        buyButton.onClick.AddListener(OnBuyButtonClicked);
    }

    public void SetItem(ShopItem shopItem)
    {
        item = shopItem;

        if (itemIcon != null && item.itemIcon != null)
            itemIcon.sprite = item.itemIcon;

        itemNameText.text = item.itemName;
        itemCostText.text = $"{item.itemCost}";

        UpdateButtonState();
    }

    public void UpdateButtonState()
    {
        bool isOwned = SkinManager.Instance.IsSkinOwned(item.itemName);
        bool isEquipped = SkinManager.Instance.IsEquipped(item.itemName);
        bool canAfford = GameManager.Instance.GetCoins() >= item.itemCost;

        if (isEquipped)
        {
            buyButtonText.text = "Equipped";
            buyButton.image.color = equippedColor;
            buyButton.interactable = false;
        }
        else if (isOwned)
        {
            buyButtonText.text = "Equip";
            buyButton.image.color = affordableColor;
            buyButton.interactable = true;
        }
        else if (canAfford)
        {
            buyButtonText.text = "Buy";
            buyButton.image.color = affordableColor;
            buyButton.interactable = true;
        }
        else
        {
            buyButtonText.text = "No coins";
            buyButton.image.color = unaffordableColor;
            buyButton.interactable = false;
        }
    }

    void OnBuyButtonClicked()
    {
        bool isOwned = SkinManager.Instance.IsSkinOwned(item.itemName);

        if (isOwned)
        {
            SkinManager.Instance.EquipSkin(item.itemName, item.skinSprite);
        }
        else
        {
            if (!GameManager.Instance.SpendCoins(item.itemCost))
            {
                Debug.Log("Not enough coins!");
                return;
            }
            SkinManager.Instance.BuySkin(item.itemName, item.skinSprite);
        }

        Shop.Instance.RefreshAllButtons();
    }
}