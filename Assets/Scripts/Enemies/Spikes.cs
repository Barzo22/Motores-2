using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] float activeTime = 1f;    
    [SerializeField] float inactiveTime = 1f;  
    [SerializeField] float timeOffset = 0f;  

    Collider2D col;
    SpriteRenderer spriteRenderer;
    float timer;
    bool isActive;

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

        if (timer <= 0)
        {
            isActive = !isActive;
            SetActive(isActive);
            timer = isActive ? activeTime : inactiveTime;
        }
    }

    void SetActive(bool active)
    {
        col.enabled = active;

        spriteRenderer.color = active ? Color.white : new Color(1, 1, 1, 0.3f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.PlayerDied();
        }
    }
}
