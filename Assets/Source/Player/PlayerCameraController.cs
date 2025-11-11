using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float lerpK;
    [SerializeField] private float angle;
    [SerializeField] private float distance;

    public Camera Camera { get { return cam; } }

    private Vector3 targetOffset;
    private Vector3 targetPosition;

    private void Awake()
    {
        cam.transform.parent = default;
    }

    private void Update()
    {
        Vector3 targetPos = targetPosition + targetOffset;

        cam.transform.position =
            Vector3.Lerp(cam.transform.position, targetPos,
            lerpK * Time.deltaTime);
    }

    public void SetTargetOffset(Vector3 targetPos)
    {
        Vector3 initialDir = Quaternion.AngleAxis(-angle, Vector3.right) * Vector3.up;
        targetOffset = initialDir * distance;

        cam.transform.rotation = Quaternion.LookRotation(-initialDir, Vector3.up);
        cam.transform.position = targetPos + targetOffset;
    }

    public void SetTargetPos(Vector3 targetPos)
    {
        targetPosition = targetPos;
    }
}