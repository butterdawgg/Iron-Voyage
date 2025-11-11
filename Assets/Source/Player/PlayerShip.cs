using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerShip : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float turnRadius = 5f;
    [SerializeField] private float movementLerpK = 5f;

    private BoxCollider boxCollider;

    public float GetMaxSpeed() {  return maxSpeed; }
    public float GetTurnRadius() { return turnRadius; }
    public float GetMovementLerpK() { return movementLerpK; }

    public BoxCollider GetBoxCollider()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider>();

        return boxCollider;
    }
}