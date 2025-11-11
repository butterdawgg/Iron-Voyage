using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnemyShipController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float turnRadius = 5f;
    [SerializeField] private float movementLerpK = 5f;
    [SerializeField] private LayerMask collisionCheckMask;

    private BoxCollider boxCollider;

    private Vector3 moveDirection;
    private float speed;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();

        transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
    }

    private void Update()
    {
        // Ensure proper input:
        moveDirection.y = 0f;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        // Box dimensions calculation:
        Vector3 boxCenter =
            transform.TransformPoint(boxCollider.center);

        Vector3 boxHalfExtents =
            Vector3.Scale(boxCollider.size * 0.5f, transform.lossyScale);

        // Collision check:
        Collider[] overlaps =
            Physics.OverlapBox(boxCenter, boxHalfExtents, transform.rotation,
            collisionCheckMask);

        foreach (Collider other in overlaps)
        {
            if (Physics.ComputePenetration(
                boxCollider, transform.position, transform.rotation,
                other, other.transform.position, other.transform.rotation,
                out Vector3 dir, out float dist))
            {
                transform.position += dir * dist;
            }
        }

        // Movement:
        Rotate(moveDirection);

        if (moveDirection.sqrMagnitude > 0.001f)
            Move(transform.forward, maxSpeed);
        else
            Move(transform.forward, 0f);
    }

    private void Rotate(Vector3 dir)
    {
        float angularSpeed = maxSpeed / turnRadius;

        float angleDiff = Vector3.SignedAngle(transform.forward, dir, Vector3.up);
        float maxTurnPerStep = angularSpeed * Time.deltaTime * Mathf.Rad2Deg;
        float turnStep = Mathf.Clamp(angleDiff, -maxTurnPerStep, maxTurnPerStep);

        Quaternion deltaRot = Quaternion.Euler(0f, turnStep, 0f);
        Vector3 newForward = deltaRot * transform.forward;
        transform.rotation = Quaternion.LookRotation(newForward, Vector3.up);
    }

    private void Move(Vector3 dir, float targetSpeed)
    {
        speed = Mathf.Lerp(speed, targetSpeed, movementLerpK * Time.deltaTime);
        transform.position += speed * Time.deltaTime * dir;
    }

    public void SetMoveDirection(Vector3 moveDir)
    {
        moveDirection = moveDir;
    }
}