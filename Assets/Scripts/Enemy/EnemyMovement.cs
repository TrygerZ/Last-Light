using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D rb;
    private Transform target;
    [SerializeField] float track;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void FixedUpdate()
    {
        if (target)
        {
            Vector2 direction =(target.position - transform.position).normalized;
            float distance = Vector2.Distance(transform.position, target.position);

            if (distance > 0.5f && distance < track)
            {
                rb.linearVelocity = direction * moveSpeed;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}
