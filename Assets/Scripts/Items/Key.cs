using UnityEngine;

public class Key : Interactable
{
    [SerializeField] string keyID = "key_1";
    [SerializeField] Door door;
    protected override void OnPlayerInteract()
    {
        door.Open();
        Destroy(gameObject);
    }
}
