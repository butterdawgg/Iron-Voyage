using UnityEngine;

public class ProjectileLight : Projectile
{
    protected override bool DetectHit(Ray ray, out RaycastHit hit, float maxDistance,
        LayerMask hitMask)
    {
        return Physics.Raycast(ray, out hit, maxDistance, hitMask);
    }

    protected override void OnUpdate() { }

    protected override void OnHit(RaycastHit hit)
    {
        if (IsFriendly)
        {
            Enemy enemy = hit.collider.GetComponentInParent<Enemy>();

            if (enemy != null)
                enemy.TakeDamage(Damage);
        }
        else
        {
            Player player = hit.collider.GetComponentInParent<Player>();

            if (player != null)
                player.TakeDamage(Damage);
        }

        transform.position = hit.point;
    }

    protected override void OnExpiry() { }
}