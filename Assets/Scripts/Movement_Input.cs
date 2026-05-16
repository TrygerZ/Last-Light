using UnityEngine;

public class Movement_Input : MonoBehaviour
{
   public float movespeed;

    private Rigidbody2D rb;
    private Transform playerBody;
    private SpriteRenderer sprite;
    public Animator animator;

    [Header("SFX")]
    [SerializeField] private AudioSource footstepAudioSource;
    [Tooltip("Footstep .mp3 clips (0-10). Played sequentially in a continuous loop while moving.")]
    [SerializeField] private AudioClip[] footstepClips;

    private bool wasMoving;
    private int currentFootstepIndex = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        playerBody = GameObject.FindGameObjectWithTag("PlayerBody").GetComponent<Transform>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        bool isMoving = Mathf.Abs(moveInput) > 0.1f;

        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        rb.linearVelocity = new Vector2(moveInput * movespeed, rb.linearVelocity.y);

        // Footstep SFX — sequential cycle 0→1→2→...→10→0→1→... while moving
        if (isMoving)
        {
            if (footstepAudioSource != null && footstepClips.Length > 0)
            {
                // If not playing anything, start the current clip
                if (!footstepAudioSource.isPlaying)
                {
                    footstepAudioSource.clip = footstepClips[currentFootstepIndex];
                    footstepAudioSource.Play();

                    // Advance to next index (loop back to 0 after last)
                    currentFootstepIndex++;
                    if (currentFootstepIndex >= footstepClips.Length)
                        currentFootstepIndex = 0;
                }
            }
        }
        else if (!isMoving && wasMoving)
        {
            if (footstepAudioSource != null && footstepAudioSource.isPlaying)
                footstepAudioSource.Stop();
            currentFootstepIndex = 0; // reset to start for next movement
        }
        wasMoving = isMoving;

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
