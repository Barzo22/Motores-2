using UnityEngine;

public class Coin : Interactable
{
    [SerializeField] int value = 1;

    protected override void OnPlayerInteract()
    {
        GameManager.Instance.AddCoins(value);
        Destroy(gameObject);
    }
}
