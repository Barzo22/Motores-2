using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop Instance;

    [SerializeField] ShopItem[] items;
    [SerializeField] ShopButton buttonPrefab;
    [SerializeField] Transform shopContent; // el Content del Scroll View

    ShopButton[] spawnedButtons;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        spawnedButtons = new ShopButton[items.Length];

        // spawneamos un botón por cada item, igual que en los apuntes
        for (int i = 0; i < items.Length; i++)
        {
            var newButton = Instantiate(buttonPrefab, shopContent);
            newButton.SetItem(items[i]);
            spawnedButtons[i] = newButton;
        }
    }

    // refresca el estado de todos los botones (después de comprar o gastar monedas)
    public void RefreshAllButtons()
    {
        foreach (var button in spawnedButtons)
            button.UpdateButtonState();
    }
}
