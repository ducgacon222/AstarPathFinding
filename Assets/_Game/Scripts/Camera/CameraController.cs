using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("---Configs---")]
    [SerializeField] private float padding = 1f;

    [Header("---References---")]
    [SerializeField] private Camera cam;

    private void Awake()
    {
        if(cam == null)
        {
            cam = GetComponent<Camera>();
        }
    }

    public void FitToGrid(float worldW, float worldH, bool offsetForUI)
    {
        float aspect = (float)Screen.width / Screen.height;

        float halfW = worldW / 2f;
        float halfH = worldH / 2f;

        float sizeByH = halfH;
        float sizeByWFull = halfW / aspect;
        float sizeByWIn2of3 = halfW / (aspect * (2f / 3f));

        float chosenRaw = Mathf.Max(sizeByH, offsetForUI ? sizeByWIn2of3 : sizeByWFull);

        bool widthConstrained = ((offsetForUI ? sizeByWIn2of3 : sizeByWFull) > sizeByH);

        float finalSize;

        if (widthConstrained)
        {
            float paddedHalfW = halfW + padding;
            float sizeByPaddedW = paddedHalfW / (aspect * (offsetForUI ? 2f / 3f : 1f));
            finalSize = Mathf.Max(sizeByPaddedW, sizeByH);
        }
        else
        {
            float paddedHalfH = halfH + padding;
            finalSize = Mathf.Max(paddedHalfH, chosenRaw);
        }

        cam.orthographicSize = finalSize;

        float visibleW = 2f * finalSize * aspect;

        float cameraX = offsetForUI ? -visibleW / 6f : 0f;

        transform.position = new Vector3(cameraX, 0f, transform.position.z);
    }


}
