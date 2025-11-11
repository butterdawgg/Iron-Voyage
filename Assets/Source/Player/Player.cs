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

    private void Awake()
    {
        CamController = GetComponent<PlayerCameraController>();
        ShipController = GetComponent<PlayerShipController>();
        GunController = GetComponent<PlayerGunController>();
        HullController = GetComponent<PlayerHullController>();

        CamController.SetTargetOffset(transform.position);
    }

    private void Update()
    {
        CamController.SetTargetPos(transform.position);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        Vector3 moveDir = new(moveInput.x, 0f, moveInput.y);

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
            return;

        Vector2 inputDir = context.ReadValue<Vector2>();

        if (inputDir.sqrMagnitude < 0.01f)
            return;

        GunController.OnAimDirectionChanged(
            new Vector3(inputDir.x, 0f, inputDir.y));
    }

    public void OnAimPosition(InputAction.CallbackContext context)
    {
        Camera cam = CamController.Camera;

        if (cam == null)
            return;

        Ray ray = cam.ScreenPointToRay(context.ReadValue<Vector2>());

        Plane plane = new (Vector3.up, Vector3.zero);

        if (!plane.Raycast(ray, out float enter))
            return;

        GunController.OnAimPositionChanged(ray.GetPoint(enter));
    }

    public void TakeDamage(float damage)
    {
        HullController.TakeDamage(damage);
    }

    public float GetHealth()
    {
        return HullController.GetHealth();
    }
}