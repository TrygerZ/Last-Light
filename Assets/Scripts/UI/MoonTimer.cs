using UnityEngine;

public class MoonTimer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform moonTransform;
    [SerializeField] private SpriteRenderer moonSprite;
    [SerializeField] private WinScreen winScreen;
    [SerializeField] private Camera mainCamera;

    [Header("Settings")]
    [SerializeField] private float totalDuration = 300f;

    [Header("Movement")]
    [Tooltip("Fraction of camera width from edges as padding (0.0-1.0). 0.1 = 10%.")]
    [SerializeField] private float horizontalPaddingFraction = 0.1f;
    [Tooltip("How high the moon arcs upward from center (in world units). 0 = straight horizontal line.")]
    [SerializeField] private float arcHeight = 2f;

    [Header("Debug (Readonly)")]
    [SerializeField] private float remainingTime;
    [Tooltip("1.0 = full time left, 0.0 = time's up")]
    [SerializeField] private float normalizedTime;
    [SerializeField] private bool isFinished;

    public float RemainingTime => remainingTime;
    public float NormalizedTime => normalizedTime;
    public bool IsFinished => isFinished;

    private float leftBound;
    private float rightBound;
    private float centerY;
    private bool hasStarted;

    private void Start()
    {
        remainingTime = totalDuration;
        isFinished = false;

        if (moonTransform == null)
        {
            Debug.LogError("MoonTimer: moonTransform is not assigned!");
            return;
        }

        if (mainCamera == null)
            mainCamera = Camera.main;

        // Set initial Z so moon is visible in front of camera
        Vector3 startPos = moonTransform.localPosition;
        startPos.z = 10f;
        moonTransform.localPosition = startPos;

        CalculateBounds();
        SetMoonPosition(1f);
    }

    private void Update()
    {
        if (isFinished || moonTransform == null) return;

        if (!hasStarted)
        {
            hasStarted = true;
            CalculateBounds();
        }

        remainingTime -= Time.deltaTime;
        normalizedTime = Mathf.Clamp01(remainingTime / totalDuration);

        // Moon moves from LEFT (normalized=1) to RIGHT (normalized=0)
        SetMoonPosition(1f - normalizedTime);

        if (remainingTime <= 0f && !isFinished)
        {
            isFinished = true;
            remainingTime = 0f;
            normalizedTime = 0f;
            SetMoonPosition(1f);

            if (winScreen != null)
                winScreen.ShowWin();

            Debug.Log("🌙 Moon Timer: Time's up! Player wins!");
        }
    }

    private void CalculateBounds()
    {
        if (mainCamera == null) return;

        // Camera viewport size in world units
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Horizontal bounds with padding
        float hPadding = cameraWidth * horizontalPaddingFraction;
        leftBound = -(cameraWidth * 0.5f) + hPadding;
        rightBound = (cameraWidth * 0.5f) - hPadding;

        // Moon stays at vertical center
        centerY = 0f;

        Debug.Log($"MoonTimer: Camera {cameraWidth:F1}x{cameraHeight:F1}, "
            + $"X:{leftBound:F1}→{rightBound:F1}, CenterY:0");
    }

    private void SetMoonPosition(float t)
    {
        // X: linear from left → right at center height
        float xPos = Mathf.Lerp(leftBound, rightBound, t);

        // Y: parabola on top of center Y
        //   t=0   → centerY + 0       = center (start)
        //   t=0.5 → centerY + arcHeight (peak)
        //   t=1   → centerY + 0       = center (finish)
        float yPos = centerY + (-4f * arcHeight * (t * (t - 1f)));

        Vector3 pos = moonTransform.localPosition;
        pos.x = xPos;
        pos.y = yPos;
        pos.z = 10f;
        moonTransform.localPosition = pos;
    }
}
