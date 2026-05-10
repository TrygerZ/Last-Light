using UnityEngine;

public class Player : MonoBehaviour
{
   public float movespeed = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * movespeed, rb.linearVelocity.y);

        if (moveInput > 0)
            sprite.flipX = true;
        else if (moveInput < 0)
            sprite.flipX = false;
    }
}
