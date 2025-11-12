using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerCameraController))]
[RequireComponent(typeof(PlayerShipController))]
[RequireComponent(typeof(PlayerGunController))]
[RequireComponent(typeof(PlayerHullController))]
public class Player : MonoBehaviour
{
    public PlayerCameraController CamController { get; private set; }
    public PlayerShipController ShipController { get; private set; }
    public PlayerGunController GunController { get; private set; }
    public PlayerHullController HullController { get; private set; }

    private PlayerInput input;
    private Vector2 aimPosInput;

    public bool IsDead { get; private set; }

    private void Awake()
    {
        CamController = GetComponent<PlayerCameraController>();
        ShipController = GetComponent<PlayerShipController>();
        GunController = GetComponent<PlayerGunController>();
        HullController = GetComponent<PlayerHullController>();

        input = GetComponent<PlayerInput>();

        CamController.SetTargetOffset(transform.position);
    }

    private void Update()
    {
        if (HullController.IsDead())
        {
            OnDeath();
        }

        CamController.SetTargetPos(transform.position);

        Vector3 aimPos = GetAimPosition(aimPosInput);

        GunController.SetAimPosition(aimPos);
    }

    private void OnDeath()
    {
        IsDead = true;

        GunController.SetCanShoot(false);
        ShipController.SetMoveDirection(Vector3.zero);
        input.enabled = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        Vector3 moveDir = new (moveInput.x, 0f, moveInput.y);

        ShipController.SetMoveDirection(moveDir);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
            GunController.SetCanShoot(true);
        else if (context.canceled)
            GunController.SetCanShoot(false);
    }

    public void OnAimDirection(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            GunController.OnAimDirectionChanged(Vector3.zero, true);

            return;
        }

        Vector2 inputDir = context.ReadValue<Vector2>();

        if (inputDir.sqrMagnitude < 0.01f)
            return;

        GunController.OnAimDirectionChanged(GetAimDirection(inputDir), false);
    }

    private Vector3 GetAimDirection(Vector2 input)
    {
        return new Vector3(input.x, 0f, input.y);
    }

    public void OnAimPosition(InputAction.CallbackContext context)
    {
        GunController.OnAimPositionChanged();

        aimPosInput = context.ReadValue<Vector2>();
    }

    private Vector3 GetAimPosition(Vector2 input)
    {
        Camera cam = CamController.Camera;

        if (cam == null)
            return Vector3.zero;

        Ray ray = cam.ScreenPointToRay(input);

        Plane plane = new (Vector3.up, Vector3.zero);

        if (!plane.Raycast(ray, out float enter))
            return Vector3.zero;

        return ray.GetPoint(enter);
    }

    public void TakeDamage(float damage)
    {
        HullController.TakeDamage(damage);
    }

    public float GetHealth()
    {
        return HullController.GetHealth();
    }

    public float GetMaxHealth()
    {
        return HullController.GetMaxHealth();
    }
}