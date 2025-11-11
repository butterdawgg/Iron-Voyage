using UnityEngine;

public class Cannon : Gun
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private float damage;
    [SerializeField] private float cooldown;
    [SerializeField] private float range;
    [SerializeField] private float spread;
    [SerializeField] private float projectileSpeed;
    [Tooltip("Min and max projectile count")]
    [SerializeField] private Vector2Int projectileCount;
    [SerializeField] private LayerMask projectileHitMask;

    private float previousShotTime;
    private bool canShoot;
    private bool isFriendly;
    private Vector3 aimDirection;
    private Vector3 aimPosition;

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(aimDirection, Vector3.up);

        if (!canShoot)
            return;

        if (Time.time - previousShotTime < cooldown)
            return;

        previousShotTime = Time.time;

        int projCount = Random.Range(projectileCount.x, projectileCount.y + 1);

        for (int i = 0; i < projCount; i++)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        float angleRad = spread * Mathf.Deg2Rad;

        // Random point on a cone around aimDirection
        Vector3 direction = Random.insideUnitCircle.normalized * Mathf.Tan(angleRad);
        direction = (aimDirection + new Vector3(direction.x, direction.y, 0)).normalized;

        Projectile.Launch(projectile, transform.position,
            direction, projectileSpeed, 0f, projectileHitMask,
            range / projectileSpeed, damage, isFriendly);
    }

    public override bool CheckVisibility(LayerMask mask)
    {
        Ray ray = new (transform.position, aimDirection);
        float dist = (transform.position - aimPosition).magnitude;

        if (dist > range)
            return false;

        if (Physics.Raycast(ray, dist, mask))
            return false;

        return true;
    }

    public override void ScaleParameters(int level)
    {
        // TODO: implement
    }

    public override void SetCanShoot(bool canShoot)
    {
        this.canShoot = canShoot;
    }

    public override void SetAim(Vector3 aimPos, Vector3 aimDir)
    {
        aimDirection = aimDir;
        aimPosition = aimPos;
    }

    public override void SetFriendly(bool friendly)
    {
        isFriendly = friendly;
    }

    public override float GetRange()
    {
        return range;
    }
}