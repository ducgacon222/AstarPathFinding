using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("---References---")]
    [SerializeField] private Camera cam;

    private void Awake()
    {
        if(cam == null)
        {
            cam = GetComponent<Camera>();
        }
    }

    public void FitToGrid(float worldWidth, float worldHeight)
    {
        transform.position = new Vector3(0f, 0f, transform.position.z);

        float halfHeight = worldHeight / 2f;
        float halfWidth = worldWidth / 2f;

        float aspect = (float)Screen.width / Screen.height;

        this.cam.orthographicSize = Mathf.Max(halfHeight, halfWidth / aspect);
    }
}
