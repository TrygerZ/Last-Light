using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D rb;
    private Transform target;
    [SerializeField] float track;

    [Header("Patrolling stats")]
    [SerializeField] float patrolTime;
    [SerializeField] float idleTime;
    [SerializeField] float leftEdge;
    [SerializeField] float rightEdge;

    private float patrolTimer;
    private float idleTimer;
    private int patrolDirection;
    private bool isIdle;
    private SpriteRenderer sprite;

    [Header("Phase-Based Speed")]
    [SerializeField] private MoonTimer moonTimer;
    [SerializeField] private float speedAwal = 3f;
    [SerializeField] private float speedTengah = 3.6f;
    [SerializeField] private float speedAkhir = 4.2f;

    private float currentMoveSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        patrolDirection = Random.Range(0, 2) == 0 ? -1 : 1;
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        if (moonTimer == null)
            moonTimer = FindFirstObjectByType<MoonTimer>();

        currentMoveSpeed = moveSpeed;
    }

    private void UpdatePhaseSpeed()
    {
        if (moonTimer == null) { currentMoveSpeed = moveSpeed; return; }

        float elapsed = moonTimer.TotalDuration - moonTimer.RemainingTime;

        if (elapsed <= 60f)
            currentMoveSpeed = speedAwal;
        else if (elapsed <= 180f)
            currentMoveSpeed = speedTengah;
        else
            currentMoveSpeed = speedAkhir;
    }

    private void FixedUpdate()
    {
        UpdatePhaseSpeed();

        if (target)
        {
            float distance = Vector2.Distance(transform.position, target.position);

            if (distance <= 0.1f)
            {
                rb.linearVelocity = Vector2.zero;
            }
            else if (distance < track)
            {
                Vector2 direction = (target.position - transform.position).normalized;
                rb.linearVelocity = direction * currentMoveSpeed;

                if (direction.x > 0)
                    sprite.flipX = true;
                else if (direction.x < 0)
                    sprite.flipX = false;
            }
            else
            {
                patrol();
            }
        }
    }
    private void patrol()
    {
        if (isIdle)
        {
            rb.linearVelocity = Vector2.zero;

            idleTimer += Time.fixedDeltaTime;

            if (idleTimer >= idleTime)
            {
                idleTimer = 0;
                isIdle = false;
                patrolDirection = Random.Range(0, 2) == 0 ? -1 : 1;
            }
        }
        else
        {
            patrolTimer += Time.fixedDeltaTime;

            if (transform.position.x <= leftEdge)
                patrolDirection = 1;
            else if (transform.position.x >= rightEdge)
                patrolDirection = -1;

            Vector2 move = new Vector2(patrolDirection, 0);
            rb.linearVelocity = move * currentMoveSpeed;

            if (move.x > 0)
                sprite.flipX = true;
            else if (move.x < 0)
                sprite.flipX = false;

            if (patrolTimer >= patrolTime)
            {
                patrolTimer = 0;
                isIdle = true;
            }
        }
    }
}
