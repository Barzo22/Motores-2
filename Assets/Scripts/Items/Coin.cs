using UnityEngine;
using System.Collections;

public class Coin : Interactable
{
    [SerializeField] float value = 1;
    void Start()
    {
        value = RemoteConfigManager.Instance != null
            ? RemoteConfigManager.Instance.CoinValue
            : value;
    }
    protected override void OnPlayerInteract()
    {
        GameManager.Instance.AddCoins(value);
        Destroy(gameObject);
    }
}
