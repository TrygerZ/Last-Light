using UnityEngine;

public class Movement_Input : MonoBehaviour
{
   public float movespeed;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        rb.linearVelocity = new Vector2(moveInput * movespeed, rb.linearVelocity.y);

        if (moveInput > 0)
            sprite.flipX = true;
        else if (moveInput < 0)
            sprite.flipX = false;
    }
}
