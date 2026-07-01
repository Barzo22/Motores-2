using UnityEngine;
using UnityEngine.SceneManagement;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance;

    string equippedSkinName = "";
    public Color CurrentParticleColor { get; private set; } = Color.white;

    [SerializeField] Color defaultParticleColor = new Color(0.937f, 0.322f, 0.404f); 

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

        float r = PlayerPrefs.GetFloat("ParticleR", defaultParticleColor.r);
        float g = PlayerPrefs.GetFloat("ParticleG", defaultParticleColor.g);
        float b = PlayerPrefs.GetFloat("ParticleB", defaultParticleColor.b);
        CurrentParticleColor = new Color(r, g, b);
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

        ShopItem[] allItems = Resources.LoadAll<ShopItem>("Items");
        foreach (ShopItem item in allItems)
        {
            if (item.itemName == skinName)
            {
                CurrentParticleColor = item.particleColor;
                PlayerPrefs.SetFloat("ParticleR", item.particleColor.r);
                PlayerPrefs.SetFloat("ParticleG", item.particleColor.g);
                PlayerPrefs.SetFloat("ParticleB", item.particleColor.b);
                break;
            }
        }

        PlayerPrefs.Save();
        ApplySkinToPlayer();
    }

    void ApplySkinToPlayer()
    {
        if (string.IsNullOrEmpty(equippedSkinName)) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        ShopItem[] allItems = Resources.LoadAll<ShopItem>("Items");
        foreach (ShopItem item in allItems)
        {
            if (item.itemName == equippedSkinName)
            {
                if (item.skinSprite != null)
                {
                    var sr = player.GetComponent<SpriteRenderer>();
                    if (sr != null) sr.sprite = item.skinSprite;
                }

                TrailRenderer trail = player.GetComponentInChildren<TrailRenderer>();
                if (trail != null)
                {
                    Gradient original = trail.colorGradient;
                    GradientColorKey[] colorKeys = original.colorKeys;

                    for (int i = 0; i < colorKeys.Length; i++)
                        colorKeys[i].color = item.particleColor;

                    Gradient newGradient = new Gradient();
                    newGradient.SetKeys(colorKeys, original.alphaKeys);
                    trail.colorGradient = newGradient;
                }

                break;
            }
        }
    }

    public void ApplyColorToParticleSystem(ParticleSystem ps)
    {
        if (ps == null) return;
        var main = ps.main;
        main.startColor = CurrentParticleColor;
    }

    public void ResetSkins()
    {
        equippedSkinName = "";
        CurrentParticleColor = defaultParticleColor;
    }

    public bool IsSkinOwned(string skinName)
    {
        return PlayerPrefs.GetInt($"skin_{skinName}", 0) == 1;
    }

    public bool IsEquipped(string skinName)
    {
        return equippedSkinName == skinName;
    }

    public void UnequipSkin()
    {
        equippedSkinName = "";
        CurrentParticleColor = defaultParticleColor;
        PlayerPrefs.SetString("EquippedSkin", "");
        PlayerPrefs.SetFloat("ParticleR", defaultParticleColor.r);
        PlayerPrefs.SetFloat("ParticleG", defaultParticleColor.g);
        PlayerPrefs.SetFloat("ParticleB", defaultParticleColor.b);
        PlayerPrefs.Save();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            TrailRenderer trail = player.GetComponentInChildren<TrailRenderer>();
            if (trail != null)
            {
                Gradient original = trail.colorGradient;
                GradientColorKey[] colorKeys = original.colorKeys;
                for (int i = 0; i < colorKeys.Length; i++)
                    colorKeys[i].color = defaultParticleColor;
                Gradient newGradient = new Gradient();
                newGradient.SetKeys(colorKeys, original.alphaKeys);
                trail.colorGradient = newGradient;
            }
        }

        if (Shop.Instance != null)
            Shop.Instance.RefreshAllButtons();
    }
}
