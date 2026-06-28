using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Sprite openSprite;
    [SerializeField] Sprite closedSprite;

    SpriteRenderer spriteRenderer;
    Collider2D col;
    bool isOpen = false;

    string doorID;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        doorID = $"door_{transform.position.x}_{transform.position.y}";

        if (GameManager.Instance.IsDoorOpen(doorID))
            SetOpen(true);
        else
            SetOpen(false);
    }

    public void Open()
    {
        GameManager.Instance.RegisterDoorOpen(doorID);
        SetOpen(true);
    }

    void SetOpen(bool open)
    {
        isOpen = open;
        spriteRenderer.sprite = open ? openSprite : closedSprite;
        col.enabled = open;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isOpen)
            GameManager.Instance.LevelComplete(GameManager.Instance.GetCoinsCollectedThisAttempt());
    }
}