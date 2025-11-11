using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

[RequireComponent(typeof(EnemyShipController))]
[RequireComponent(typeof(EnemyGunController))]
public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Maximum HP")]
    [SerializeField] private float maxHealth;
    [Tooltip("Percentage of damage absorbed")]
    [SerializeField] private float damageAbsorption;
    [Tooltip("Child objects to be destroyed on death")]
    [SerializeField] private GameObject[] destroyOnDeath;
    [Tooltip("Visual effect to be played on death")]
    [SerializeField] private VisualEffect deathVfx;

    [Header("Attacking")]
    [Tooltip("Leave empty to make the enemy a kamikaze")]
    [SerializeField] private Gun[] guns;

    [Tooltip("Distance to the player at which the enemy explodes")]
    [SerializeField, ConditionalHide("guns", false, true)]
    private float blastDistance;

    [Tooltip("AOE of the explosion")]
    [SerializeField, ConditionalHide("guns", false, true)]
    private float blastRadius;

    [Tooltip("Instantaneous damage of the explosion")]
    [SerializeField, ConditionalHide("guns", false, true)]
    private float blastDamage;

    [Header("Movement")]
    [Tooltip("Clear line of sight distance to the player at which the enemy stops")]
    [SerializeField, ConditionalHide("guns", false, false)]
    private float stoppingDistance = 10f;

    [Header("Obstacle avoidance")]
    [Tooltip("Min distance from obstacle")]
    [SerializeField] private float radius = 0.5f;
    [Tooltip("Determines if can fit under an obstacle")]
    [SerializeField] private float height = 2f;

    private NavMeshAgent agent;
    private EnemyShipController shipController;
    private EnemyGunController gunController;

    private float health;
    private Vector3 targetPosition;

    public bool IsDead { get; private set; }

    private void Start()
    {
        agent = gameObject.AddComponent<NavMeshAgent>();
        shipController = GetComponent<EnemyShipController>();
        gunController = GetComponent<EnemyGunController>();

        health = maxHealth;

        // Use agent only for pathfinding
        agent.updatePosition = false;
        agent.updateRotation = false;

        agent.radius = radius;
        agent.height = height;

        agent.autoBraking = false;
        agent.autoRepath = true;
    }

    private void Update()
    {
        // Health logic:

        if (IsDead)
            return;

        if (health <= 0f)
        {
            OnDeath();

            return;
        }

        // Path logic:
        agent.SetDestination(targetPosition);

        Vector3 nextCorner = agent.path.corners.Length > 1
            ? agent.path.corners[1]
            : targetPosition;

        Vector3 toTarget = targetPosition - transform.position;
        Vector3 toNextCorner = nextCorner - transform.position;

        bool sameDirection = Vector3.Angle(toTarget, toNextCorner) < 5f;
        float distanceToTarget = toTarget.magnitude;

        bool shouldStop = sameDirection && distanceToTarget < stoppingDistance;

        Vector3 desiredVel = agent.desiredVelocity;

        if (!shouldStop && desiredVel.sqrMagnitude > 0.01f)
            shipController.SetMoveDirection(desiredVel.normalized);
        else
            shipController.SetMoveDirection(Vector3.zero);

        // Keep agent position synced with actual transform
        agent.nextPosition = transform.position;

        // Guns aiming
        Vector3 targetDir = (targetPosition - transform.position).normalized;
        gunController.ControlGuns(guns, targetPosition, targetDir);
    }

    private void OnDeath()
    {
        IsDead = true;

        gunController.DisableGuns(guns);
        shipController.SetMoveDirection(Vector3.zero);

        foreach (var obj in destroyOnDeath)
        {
            Destroy(obj);
        }

        if (deathVfx != null)
            deathVfx.Play();

        Destroy(gameObject, 5f);
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    public void TakeDamage(float damage)
    {
        float factor = (100f - Mathf.Clamp(damageAbsorption, 0f, 100f)) / 100f;

        health -= damage * factor;

        health = Mathf.Clamp(health, 0f, maxHealth);
    }

    public float GetHealth()
    {
        return health;
    }
}