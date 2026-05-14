using UnityEngine;

public class Movement_Input : MonoBehaviour
{
   public float movespeed;

    private Rigidbody2D rb;
    private Transform playerBody;
    private SpriteRenderer sprite;
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        playerBody = GameObject.FindGameObjectWithTag("PlayerBody").GetComponent<Transform>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        rb.linearVelocity = new Vector2(moveInput * movespeed, rb.linearVelocity.y);

        if (moveInput > 0)
        {
            sprite.flipX = true;
            playerBody.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveInput < 0)
        {
            sprite.flipX = false;
            playerBody.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
