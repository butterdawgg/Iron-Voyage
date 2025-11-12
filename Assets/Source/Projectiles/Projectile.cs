using UnityEngine;
using UnityEngine.VFX;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject[] destroyOnHit;
    [SerializeField] private VisualEffect hitVfx;
    [SerializeField] private VisualEffect shootVfx;
    [SerializeField] private float shootVfxOffset;

    private LayerMask hitMask;

    private Vector3 velocity;
    private float gravity;

    private float lifetime;
    private float launchTime;

    protected float Damage { get; private set; }
    protected bool IsFriendly { get; private set; }
    protected bool IsDead { get; private set; }

    public static void Launch(Projectile prototype, Vector3 position,
        Vector3 direction, float speed, float gravity, LayerMask hitMask,
        float lifetime, float damage, bool isFriendly)
    {
        Projectile proj = Instantiate(prototype, position,
            Quaternion.LookRotation(direction), (Transform)default);

        proj.velocity = proj.transform.forward * speed;
        proj.gravity = gravity;

        proj.hitMask = hitMask;

        proj.lifetime = lifetime;
        proj.launchTime = Time.time;

        proj.Damage = damage;
        proj.IsFriendly = isFriendly;

        proj.IsDead = false;

        proj.shootVfx.transform.parent = default;
        proj.shootVfx.transform.position += direction * proj.shootVfxOffset;
        proj.shootVfx.Play();
    }

    private void Update()
    {
        if (IsDead)
            return;

        if (Time.time - launchTime > lifetime)
        {
            Expire();

            return;
        }

        velocity += gravity * Time.deltaTime * Vector3.down;

        Vector3 displacement = velocity * Time.deltaTime;
        float dist = displacement.magnitude;

        Ray ray = new(transform.position, velocity);
        if (DetectHit(ray, out RaycastHit hit, dist, hitMask))
        {
            Hit(hit);

            return;
        }

        transform.position += displacement;

        OnUpdate();
    }

    private void Hit(RaycastHit hit)
    {
        OnHit(hit);

        Die();
    }

    private void Expire()
    {
        OnExpiry();

        Die();
    }

    private void Die()
    {
        IsDead = true;

        foreach (var obj in destroyOnHit)
            Destroy(obj);

        if (hitVfx != null)
            hitVfx.Play();

        if (shootVfx != null)
            Destroy(shootVfx, 5f);

        Destroy(gameObject, 5f);
    }

    protected abstract bool DetectHit(Ray ray, out RaycastHit hit, float maxDistance,
        LayerMask hitMask);

    protected abstract void OnUpdate();
    protected abstract void OnHit(RaycastHit hit);
    protected abstract void OnExpiry();
}