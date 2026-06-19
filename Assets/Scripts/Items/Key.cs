using UnityEngine;

public class Key : Interactable
{
    [SerializeField] Door door;
    string keyID;

    void Start()
    {
        keyID = $"key_{transform.position.x}_{transform.position.y}";

        if (GameManager.Instance.HasKey(keyID))
            gameObject.SetActive(false);
    }

    protected override void OnPlayerInteract()
    {
        GameManager.Instance.CollectKey(keyID);
        door.Open();
        gameObject.SetActive(false);
    }
}