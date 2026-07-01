using UnityEngine;

public class Key : Interactable
{
    [SerializeField] Door door;
    [SerializeField] ParticleSystem collectEffect;
    [SerializeField] AudioClip collectSound;

    string keyID;

    void Start()
    {
        keyID = $"key_{transform.position.x}_{transform.position.y}";

        if (GameManager.Instance.HasKey(keyID))
            gameObject.SetActive(false);
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

        GameManager.Instance.CollectKey(keyID);
        door.Open();
        gameObject.SetActive(false);
    }
}