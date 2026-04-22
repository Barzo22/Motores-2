using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] string requiredKeyID = "key_1";
    [SerializeField] float checkRadius = 5f;

    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void OnEnable()
    {
        PlayerMovement.OnPlayerMoved += CheckForPlayer;
    }

    void OnDisable()
    {
        PlayerMovement.OnPlayerMoved -= CheckForPlayer;
    }

    void CheckForPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= checkRadius && GameManager.Instance.HasKey(requiredKeyID))
        {
            GameManager.Instance.UseKey(requiredKeyID);
            Destroy(gameObject);
        }
    }
}