using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] float activeTime = 1f;
    [SerializeField] float inactiveTime = 1f;
    [SerializeField] float timeOffset = 0f;
    [SerializeField] Color activeColor = Color.red;

    // cuánto tiempo antes de activarse empieza el warning
    [SerializeField] float warningTime = 1f;
    // velocidad del parpadeo
    [SerializeField] float blinkSpeed = 8f;

    Collider2D col;
    SpriteRenderer spriteRenderer;
    float timer;
    bool isActive;
    bool isWarning;

    void Start()
    {
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = timeOffset;
        isActive = true;
        SetActive(true);
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (!isActive)
        {
            // si estamos cerca de activarnos, mostramos el warning
            if (timer <= warningTime)
            {
                isWarning = true;
                // parpadeamos entre transparente y rojo usando un seno
                float alpha = (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f;
                spriteRenderer.color = new Color(activeColor.r, activeColor.g, activeColor.b, alpha);
            }
        }

        if (timer <= 0)
        {
            isWarning = false;
            isActive = !isActive;
            SetActive(isActive);
            timer = isActive ? activeTime : inactiveTime;
        }
    }
    void SetActive(bool active)
    {
        col.enabled = active;
        spriteRenderer.color = active ? activeColor : new Color(1, 1, 1, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            GameManager.Instance.PlayerDied();
    }
}