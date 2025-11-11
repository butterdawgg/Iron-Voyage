using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    [SerializeField] private Transform shipPivot;
    [SerializeField] private LayerMask collisionCheckMask;

    private PlayerShip ship;

    private Vector3 moveDirection;
    private float speed;

    private void Update()
    {
        BoxCollider boxCollider = ship.GetBoxCollider();
        float maxSpeed = ship.GetMaxSpeed();

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
        float maxSpeed = ship.GetMaxSpeed();
        float turnRadius = ship.GetTurnRadius();

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
        float movementLerpK = ship.GetMovementLerpK();

        speed = Mathf.Lerp(speed, targetSpeed, movementLerpK * Time.deltaTime);
        transform.position += speed * Time.deltaTime * dir;
    }

    public void SetMoveDirection(Vector3 moveDir)
    {
        moveDirection = moveDir;
    }

    public void Add(PlayerShip shipPrefab)
    {
        ship = Instantiate(shipPrefab.gameObject,
            shipPivot.position, shipPivot.rotation,
            shipPivot).GetComponent<PlayerShip>();
    }
}