using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (transform.position.x < leftBoundary)isLeft = true;
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void FixedUpdate()
    {
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
                rb.linearVelocity = direction * moveSpeed;
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

                patrolDirection = Random.Range(0, 2) == 0 ? -1 : 1; // for some reason (0,2) means 0 <= random < 2, so only 0 and 1
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

            rb.linearVelocity = move * moveSpeed;

            if (patrolTimer >= patrolTime)
            {
                patrolTimer = 0;

                isIdle = true;
            }
        }
    }
}
