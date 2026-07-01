using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop Instance;

    [SerializeField] ShopItem[] items;
    [SerializeField] ShopButton buttonPrefab;
    [SerializeField] Transform shopContent; 

    ShopButton[] spawnedButtons;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        spawnedButtons = new ShopButton[items.Length];

        for (int i = 0; i < items.Length; i++)
        {
            var newButton = Instantiate(buttonPrefab, shopContent);
            newButton.SetItem(items[i]);
            spawnedButtons[i] = newButton;
        }
    }

    public void RefreshAllButtons()
    {
        foreach (var button in spawnedButtons)
            button.UpdateButtonState();
    }
}
