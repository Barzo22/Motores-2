using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float threshold = 100f;
    [SerializeField] float deadZone = 0.2f;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] float raycastDistance = 0.1f;

    Vector2 swipeInitialPosition;
    bool swipeCalculated = false;
    bool isMoving = false;
    Vector2 moveDirection;

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
    }
    void MoveUntilWall()
    {
        float playerHalfSize = 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, raycastDistance, wallLayer);

        if (hit.collider != null)
        {
            float distanceToWall = hit.distance - playerHalfSize;

            if (distanceToWall > 0)
            {
                transform.Translate(moveDirection * distanceToWall);
            }

            isMoving = false;
        }
        else
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
    }
}


