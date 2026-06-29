using UnityEngine;
using System.Collections;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float threshold = 100f;
    [SerializeField] float deadZone = 0.2f;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] float raycastDistance = 0.1f;

    [SerializeField] float squashAmount = 0.6f;
    [SerializeField] float stretchAmount = 1.4f;
    [SerializeField] float squashDuration = 0.08f;
    [SerializeField] float returnDuration = 0.15f;

    [Header("Effects")]
    [SerializeField] ParticleSystem wallHitVertical;
    [SerializeField] ParticleSystem wallHitHorizontal;
    [SerializeField] AudioClip wallHitSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioSource audioSource;

    Vector2 swipeInitialPosition;
    bool swipeCalculated = false;
    bool isMoving = false;
    Vector2 moveDirection;
    bool eventFired = false;

    Vector3 originalScale;

    public static event System.Action OnPlayerMoved;
    public static PlayerMovement Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        originalScale = transform.localScale;

        if (RemoteConfigManager.Instance != null)
            moveSpeed = RemoteConfigManager.Instance.MoveSpeed;

        RemoteConfigManager.OnConfigLoaded += ApplyRemoteConfig;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void OnDestroy()
    {
        RemoteConfigManager.OnConfigLoaded -= ApplyRemoteConfig;
        if (Instance == this) Instance = null;
    }

    void ApplyRemoteConfig()
    {
        moveSpeed = RemoteConfigManager.Instance.MoveSpeed;
        RemoteConfigManager.OnConfigLoaded -= ApplyRemoteConfig;
    }

    void Update()
    {
        if (isMoving)
        {
            MoveUntilWall();
            return;
        }

        if (Input.touchCount > 0)
        {
            var touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                swipeInitialPosition = touch.position;
                swipeCalculated = false;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (swipeCalculated || Vector2.Distance(swipeInitialPosition, touch.position) < threshold) return;
                CalculateSwipe(touch.position);
                swipeCalculated = true;
            }
        }
    }

    void CalculateSwipe(Vector2 position)
    {
        var dir = position - swipeInitialPosition;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.normalized.x > deadZone)
                StartMoving(Vector2.right);
            else if (dir.normalized.x < -deadZone)
                StartMoving(Vector2.left);
        }
        else
        {
            if (dir.normalized.y > deadZone)
                StartMoving(Vector2.up);
            else if (dir.normalized.y < -deadZone)
                StartMoving(Vector2.down);
        }
    }

    void StartMoving(Vector2 direction)
    {
        if (isMoving) return;
        moveDirection = direction;
        isMoving = true;
        eventFired = false;
    }

    void MoveUntilWall()
    {
        float playerHalfSize = 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, raycastDistance, wallLayer);

        if (hit.collider != null)
        {
            float moveDistance = Mathf.Max(0f, hit.distance - playerHalfSize);
            transform.position = (Vector2)transform.position + moveDirection * moveDistance;
            isMoving = false;

            if (wallHitVertical != null && wallHitHorizontal != null)
            {
                bool isHorizontalMove = Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y);
                ParticleSystem effect = isHorizontalMove ? wallHitHorizontal : wallHitVertical;
                Vector2 hitPoint = (Vector2)transform.position + moveDirection * 0.2f;
                ParticleSystem ps = Instantiate(effect, hitPoint, effect.transform.rotation);

                // aplicamos el color de la skin actual desde el SkinManager
                if (SkinManager.Instance != null)
                {
                    var main = ps.main;
                    main.startColor = SkinManager.Instance.CurrentParticleColor;
                }

                ps.Play();
            }

            PlaySFX(wallHitSound);
            StartCoroutine(SquashAndStretch(moveDirection));

            if (moveDistance > 0.01f && !eventFired)
            {
                eventFired = true;
                OnPlayerMoved?.Invoke();
            }
        }
        else
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

            if (!eventFired)
            {
                eventFired = true;
                OnPlayerMoved?.Invoke();
            }
        }
    }

    public void PlayDeathSound()
    {
        if (deathSound == null) return;
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
    }

    void PlaySFX(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;
        audioSource.PlayOneShot(clip);
    }

    IEnumerator SquashAndStretch(Vector2 direction)
    {
        bool isHorizontal = Mathf.Abs(direction.x) > Mathf.Abs(direction.y);

        Vector3 squashedScale = isHorizontal
            ? new Vector3(originalScale.x * squashAmount, originalScale.y * stretchAmount, originalScale.z)
            : new Vector3(originalScale.x * stretchAmount, originalScale.y * squashAmount, originalScale.z);

        float elapsed = 0f;
        while (elapsed < squashDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, squashedScale, elapsed / squashDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = squashedScale;

        elapsed = 0f;
        while (elapsed < returnDuration)
        {
            transform.localScale = Vector3.Lerp(squashedScale, originalScale, elapsed / returnDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale;
    }
}