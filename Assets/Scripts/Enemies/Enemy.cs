using UnityEngine;
using System.Collections;

public class TurnEnemy : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] bool loop = true;
    [SerializeField] float enemySpeed = 5f;
    [SerializeField] float flashDuration = 0.15f;

    int currentWaypoint = 0;
    bool movingForward = true;
    bool isMoving = false;

    SpriteRenderer spriteRenderer;
    Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        if (RemoteConfigManager.Instance != null)
            enemySpeed = RemoteConfigManager.Instance.EnemySpeed;

        RemoteConfigManager.OnConfigLoaded += ApplyRemoteConfig;
    }

    void OnDestroy()
    {
        RemoteConfigManager.OnConfigLoaded -= ApplyRemoteConfig;
    }

    void ApplyRemoteConfig()
    {
        enemySpeed = RemoteConfigManager.Instance.EnemySpeed;
        RemoteConfigManager.OnConfigLoaded -= ApplyRemoteConfig;
    }

    void OnEnable()
    {
        PlayerMovement.OnPlayerMoved += MoveToNextWaypoint;
    }

    void OnDisable()
    {
        PlayerMovement.OnPlayerMoved -= MoveToNextWaypoint;
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0 || isMoving) return;
        StartCoroutine(MoveCoroutine(waypoints[currentWaypoint].position));
        AdvanceWaypoint();
    }

    IEnumerator MoveCoroutine(Vector2 target)
    {
        isMoving = true;

        // flasheamos a blanco para que contraste con cualquier color base del enemigo
        StartCoroutine(FlashCoroutine());

        while (Vector2.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, enemySpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = target;
        isMoving = false;
    }

    IEnumerator FlashCoroutine()
    {
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    void AdvanceWaypoint()
    {
        if (loop)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
        else
        {
            if (movingForward)
            {
                currentWaypoint++;
                if (currentWaypoint >= waypoints.Length - 1)
                    movingForward = false;
            }
            else
            {
                currentWaypoint--;
                if (currentWaypoint <= 0)
                    movingForward = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            GameManager.Instance.PlayerDied();
    }
}