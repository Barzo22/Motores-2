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

        coinID = $"coin_{transform.position.x}_{transform.position.y}";

        if (GameManager.Instance.IsCoinCollected(coinID))
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
        GameManager.Instance.CollectCoin(coinID, value);
        gameObject.SetActive(false);
    }
}