using UnityEngine;

public class Key : Interactable
{
    [SerializeField] string keyID = "key_1";

    protected override void OnPlayerInteract()
    {
        GameManager.Instance.CollectKey(keyID);
        Destroy(gameObject);
    }
}
