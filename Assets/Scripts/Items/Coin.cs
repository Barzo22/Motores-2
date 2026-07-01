using UnityEngine;

public class Coin : Interactable
{
    [SerializeField] float value = 1;
    [SerializeField] ParticleSystem collectEffect;
    [SerializeField] AudioClip collectSound;

    string coinID;

    void Start()
    {
        if (RemoteConfigManager.Instance != null)
            value = RemoteConfigManager.Instance.CoinValue;

        RemoteConfigManager.OnConfigLoaded += ApplyRemoteConfig;

        coinID = $"coin_{transform.position.x}_{transform.position.y}";

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
        if (collectEffect != null)
        {
            ParticleSystem ps = Instantiate(collectEffect, transform.position, Quaternion.identity);
            ps.Play();
        }

        if (collectSound != null)
            VolumeManager.Instance?.PlaySFXAtVolume(collectSound);

        GameManager.Instance.AddCoins(coinID, value);
        gameObject.SetActive(false);
    }
}