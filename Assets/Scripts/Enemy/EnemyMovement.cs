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
    [SerializeField] float leftBoundary;
    [SerializeField] float rightBoundary;
    [SerializeField] float leftEdge;
    [SerializeField] float rightEdge;

    private bool isLeft;
    private float patrolTimer;
    private float idleTimer;
    private int patrolDirection;
    private bool isIdle;
    private SpriteRenderer sprite;

    [Header("Phase-Based Speed (Hardcoded)")]
    [SerializeField] private MoonTimer moonTimer;
    [SerializeField] private float speedAwal = 3f;
    [SerializeField] private float speedTengah = 3.6f;
    [SerializeField] private float speedAkhir = 4.2f;

    private float currentMoveSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        if (transform.position.x < leftBoundary)isLeft = true;
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

        if (elapsed <= 60f)           // 00:00 - 01:00 → Awal
            currentMoveSpeed = speedAwal;
        else if (elapsed <= 180f)     // 01:01 - 03:00 → Tengah
            currentMoveSpeed = speedTengah;
        else                          // 03:01 - 05:00 → Akhir
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

            if ((transform.position.x >= leftBoundary && isLeft) || (transform.position.x >= rightEdge && !isLeft))
            {
                patrolDirection = -1;
            }

            if ((transform.position.x <= rightBoundary && !isLeft) || (transform.position.x <= leftEdge && isLeft))
            {
                patrolDirection = 1;
            }

            Vector2 move = new Vector2(patrolDirection, 0);

            rb.linearVelocity = move * currentMoveSpeed;

            if (move.x > 0)
            {
                sprite.flipX = true;
            }
            else if (move.x < 0)
            {
                sprite.flipX = false;
            }

            if (patrolTimer >= patrolTime)
            {
                patrolTimer = 0;
                isIdle = true;
            }
        }
    }
}
