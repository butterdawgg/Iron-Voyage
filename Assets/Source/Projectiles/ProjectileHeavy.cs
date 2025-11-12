using UnityEngine;

public class ProjectileHeavy : Projectile
{
    [SerializeField] private float blastRadius;

    protected override bool DetectHit(Ray ray, out RaycastHit hit, float maxDistance,
        LayerMask hitMask)
    {
        return Physics.Raycast(ray, out hit, maxDistance, hitMask);
    }

    protected override void OnUpdate() { }

    protected override void OnHit(RaycastHit hit)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (var collider in colliders)
        {
            if (IsFriendly)
            {
                Enemy enemy = collider.GetComponentInParent<Enemy>();

                if (enemy != null)
                    enemy.TakeDamage(Damage);
            }
            else
            {
                Player player = collider.GetComponentInParent<Player>();

                if (player != null)
                {
                    player.TakeDamage(Damage);
                    break;
                }
            }
        }

        transform.position = hit.point;
    }

    protected override void OnExpiry() { }
}
