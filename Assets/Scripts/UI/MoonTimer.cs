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
    [SerializeField] private float horizontalPaddingFraction = 0.1f;
    [SerializeField] private float arcHeight = 2f;

    [Header("Debug (Readonly)")]
    [SerializeField] private float remainingTime;
    [SerializeField] private float normalizedTime;
    [SerializeField] private bool isFinished;

    public float TotalDuration => totalDuration;
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

        SetMoonPosition(1f - normalizedTime);

        if (remainingTime <= 0f && !isFinished)
        {
            isFinished = true;
            remainingTime = 0f;
            normalizedTime = 0f;
            SetMoonPosition(1f);

            if (AudioManager.Instance != null)
                AudioManager.Instance.StopMusic();

            if (winScreen != null)
                winScreen.ShowWin();
        }
    }

    private void CalculateBounds()
    {
        if (mainCamera == null) return;

        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        float hPadding = cameraWidth * horizontalPaddingFraction;
        leftBound = -(cameraWidth * 0.5f) + hPadding;
        rightBound = (cameraWidth * 0.5f) - hPadding;

        centerY = 0f;
    }

    private void SetMoonPosition(float t)
    {
        float xPos = Mathf.Lerp(leftBound, rightBound, t);
        float yPos = centerY + (-4f * arcHeight * (t * (t - 1f)));

        Vector3 pos = moonTransform.localPosition;
        pos.x = xPos;
        pos.y = yPos;
        pos.z = 10f;
        moonTransform.localPosition = pos;
    }
}
