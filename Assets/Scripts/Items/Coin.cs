using UnityEngine;

public class Coin : Interactable
{
    [SerializeField] float value = 1;
    string coinID;

    void Start()
    {
        if (RemoteConfigManager.Instance != null)
            value = RemoteConfigManager.Instance.CoinValue;

        RemoteConfigManager.OnConfigLoaded += ApplyRemoteConfig;

        // generamos el ID por posición
        coinID = $"coin_{transform.position.x}_{transform.position.y}";

        // si ya la agarramos en este intento, la desactivamos
        if (GameManager.Instance.IsCoinCollectedThisAttempt(coinID))
            gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        RemoteConfigManager.OnConfigLoaded -= ApplyRemoteConfig;
    }

    void ApplyRemoteConfig()
    {
        value = RemoteConfigManager.Instance.CoinValue;
        RemoteConfigManager.OnConfigLoaded -= ApplyRemoteConfig;
    }

    protected override void OnPlayerInteract()
    {
        GameManager.Instance.AddCoins(coinID, value);
        gameObject.SetActive(false);
    }
}