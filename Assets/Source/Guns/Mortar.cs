using UnityEngine;

public class Mortar : Gun
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private float damage = 50f;
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private float range = 30f;
    [SerializeField] private float arcHeight = 3f;
    [SerializeField] private float flightTime = 1f;
    [SerializeField] private LayerMask projectileHitMask;

    private float previousShotTime;
    private bool canShoot;
    private bool isFriendly;
    private Vector3 aimPosition;

    private void Update()
    {
        if (!canShoot)
            return;

        if (Time.time - previousShotTime < cooldown)
            return;

        previousShotTime = Time.time;

        Shoot();
    }

    private void Shoot()
    {
        float gravity = 8 * arcHeight / (flightTime * flightTime);
        Vector3 dir = (aimPosition - transform.position);
        Vector3 velocity = new (dir.x / flightTime,
            4 * arcHeight / flightTime, dir.z / flightTime);

        Projectile.Launch(projectile, transform.position, velocity.normalized,
            velocity.magnitude, gravity, projectileHitMask, flightTime + 1f,
            damage, isFriendly);
    }

    public override bool CheckVisibility(LayerMask mask)
    {
        float dist = Vector3.Distance(transform.position, aimPosition);
        return dist <= range;
    }

    public override void SetCanShoot(bool canShoot)
    {
        this.canShoot = canShoot;
    }

    public override void SetAim(Vector3 aimPos, Vector3 aimDir)
    {
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

    public override bool DrawAimPosCursor()
    {
        return true;
    }

    public override bool DrawAimDirCursor()
    {
        return false;
    }
}