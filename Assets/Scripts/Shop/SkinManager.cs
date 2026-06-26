using UnityEngine;
using UnityEngine.SceneManagement;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance;

    string equippedSkinName = "";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        equippedSkinName = PlayerPrefs.GetString("EquippedSkin", "");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplySkinToPlayer();
    }

    public void BuySkin(string skinName, Sprite skinSprite)
    {
        PlayerPrefs.SetInt($"skin_{skinName}", 1);
        PlayerPrefs.Save();
        EquipSkin(skinName, skinSprite);
    }

    public void EquipSkin(string skinName, Sprite skinSprite)
    {
        equippedSkinName = skinName;
        PlayerPrefs.SetString("EquippedSkin", skinName);
        PlayerPrefs.Save();
        ApplySkinToPlayer();
    }

    // resetea todas las skins, se llama al borrar el progreso
    public void ResetSkins()
    {
        equippedSkinName = "";
        // aplicamos el sprite default al jugador si está en escena
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // no hacemos nada con el sprite, el jugador vuelve al default
            // cuando tu compañero agregue las skins esto se puede expandir
        }
    }

    void ApplySkinToPlayer()
    {
        if (string.IsNullOrEmpty(equippedSkinName)) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        ShopItem[] allItems = Resources.LoadAll<ShopItem>("Items");
        foreach (ShopItem item in allItems)
        {
            if (item.itemName == equippedSkinName && item.skinSprite != null)
            {
                var sr = player.GetComponent<SpriteRenderer>();
                if (sr != null) sr.sprite = item.skinSprite;
                break;
            }
        }
    }

    public bool IsSkinOwned(string skinName)
    {
        return PlayerPrefs.GetInt($"skin_{skinName}", 0) == 1;
    }

    public bool IsEquipped(string skinName)
    {
        return equippedSkinName == skinName;
    }
}
