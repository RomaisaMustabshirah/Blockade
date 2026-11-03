using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFit : MonoBehaviour
{
    [Header("Play area in world units (the part that must always be visible)")]
    public float targetWidth = 10f;
    public float targetHeight = 18f;

    Camera cam;
    int lastWidth, lastHeight;

    void Awake()
    {
        cam = GetComponent<Camera>();
        Fit();
    }

    void Update()
    {
        // Re-fit if the window size or phone orientation changes
        if (Screen.width != lastWidth || Screen.height != lastHeight)
            Fit();
    }

    void Fit()
    {
        lastWidth = Screen.width;
        lastHeight = Screen.height;

        float screenAspect = (float)Screen.width / Screen.height;

        float sizeToFitHeight = targetHeight / 2f;
        float sizeToFitWidth = targetWidth / (2f * screenAspect);

        // Use the larger one so both width and height always fit
        cam.orthographicSize = Mathf.Max(sizeToFitHeight, sizeToFitWidth);
    }
}